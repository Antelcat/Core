using System.Diagnostics;
using System.Text;
using Antelcat.Core.Interface.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Antelcat.Core.Implements.Loggers;

internal class AntelcatLogger : LoggerConfig, IAntelcatLogger
{
    public AntelcatLogger(IAntelcatLoggerFactory factory, string category)
    {
        Category = category;
        (factory as AntelcatLoggerFactory)!.Initialize(this);
        OnLog += log => File.AppendAllTextAsync(LogFile, log);
    }

    public AntelcatLogger(IAntelcatLoggerFactory factory) : this(factory,
        GetCaller() ?? string.Empty)
    {

    }

    private static string? GetCaller()
    {
        var    frames = new StackTrace(2, false).GetFrames().Reverse();
        string? ret = null;
        foreach (var frame in frames)
        {
            var method = frame.GetMethod();
            if (method?.Name is nameof(IServiceProvider.GetService) or nameof(ServiceProviderServiceExtensions.GetRequiredService))
            {
                return ret;
            }

            ret = frame.GetMethod()?.DeclaringType?.FullName;
        }
        return ret;
    }

    #region Fields

    protected override bool OutputConsole
    {
        get => outputConsole;
        set
        {
            if (value == outputConsole) return;
            if (value) OnLog += OutputToConsole;
            else OnLog       -= OutputToConsole;
            outputConsole = value;
        }
    }

    private bool outputConsole;

    private string LogDirectory
    {
        get
        {
            if (!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }

            return Directory;
        }
    }

    private string LogFile
    {
        get
        {
            var log = Path.Combine(LogDirectory, NamingFormat(DateTime.Now));
            if (!File.Exists(log))
            {
                File.Create(log).Close();
            }

            return log;
        }
    }

    private string Category { get; set; }

    protected override LogFormat Format
    {
        get => format;
        set
        {
            logGenerator = value switch
            {
                LogFormat.Plain => FormatExtension.LogPlain,
                LogFormat.Json  => FormatExtension.LogJson,
                _               => logGenerator
            };

            format = value;
        }
    }

    private LogFormat format;

    #endregion

    #region Methods

    private string FormatKey(string content) => $"{Prefix} {content} {Suffix}";

    #endregion

    private static StackFrame? GetCallerFrame(int deepness)
    {
        var frames = new StackTrace(deepness, true).GetFrames();
        return frames.FirstOrDefault(x => x.GetMethod()?.Name.StartsWith("Log") is false);
    }

    private Func<LogLevel,
        EventId,
        StackFrame?,
        string,
        string,
        Func<string, string>, string> logGenerator = FormatExtension.LogPlain;

    private event Func<string, Task> OnLog;

    private static Task OutputToConsole(string log)
    {
        Console.WriteLine(log);
        return Task.CompletedTask;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        LogInternal(GetCallerFrame(2), logLevel, eventId, formatter(state, exception));
    }

    public async void LogInternal(StackFrame? frame,
        LogLevel logLevel,
        EventId eventId,
        string content)
    {
        await OnLog(logGenerator(logLevel, eventId, frame, Category, content, FormatKey));
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return new DisposeTrigger(() =>
        {
            Log(LogLevel.Trace, new EventId(-1, "Anonymous"), state, null, (s, _) => $"{s}");
        });
    }

    internal class DisposeTrigger(Action? action) : IDisposable
    {
        public void Dispose()
        {
            action?.Invoke();
        }
    }

    private static class FormatExtension
    {
        internal static string LogPlain(LogLevel logLevel,
            EventId eventId,
            StackFrame? stack,
            string category,
            string content,
            Func<string, string> regionFormat)
        {
            var method = stack?.GetMethod();
            var sb     = new StringBuilder();
            sb
                .AppendLine(regionFormat($"等级 : [ {logLevel} ], 时间: [ {DateTime.Now} ]"))
                .AppendLine(regionFormat($"事件 : [ {eventId} ]"))
                .AppendLine(regionFormat($"模块 : [ {method?.Module.Assembly.FullName} ]"))
                .AppendLine(regionFormat($"类型 : [ {category} ]"));
            if (stack != null)
            {
                sb
                    .AppendLine(regionFormat($"文件 : [ \"{stack.GetFileName()}\" ], 行: {stack.GetFileLineNumber()}"))
                    .AppendLine(regionFormat($"方法 : [ {method} ]"));
            }

            sb
                .AppendLine(content)
                .AppendLine("\n");
            return sb.ToString();
        }

        internal static string LogJson(LogLevel logLevel,
            EventId eventId,
            StackFrame? stack,
            string category,
            string content,
            Func<string, string> _)
        {
            var method = stack?.GetMethod();
            var sb     = new StringBuilder("{\n");
            var module = method?.Module.Assembly.FullName;
            sb
                .AppendLine($"    \"{nameof(logLevel)}\" : \"{logLevel}\" , \"time\" : \"{DateTime.Now}\" ,")
                .AppendLine($"    \"{nameof(eventId)}\"  : \"{eventId}\" ,")
                .AppendLine($"    \"{nameof(module)}\"   : \"{module}\" ,")
                .AppendLine($"    \"{nameof(category)}\" : \"{category}\" ,");
            if (stack != null)
            {
                var file = stack.GetFileName()?.Replace('\\', '/');
                sb
                    .AppendLine($"    \"{nameof(file)}\"     : \"{file}\" , \"row\": {stack.GetFileLineNumber()} ,")
                    .AppendLine($"    \"{nameof(method)}\"   : \"{method}\" ,");
            }

            sb
                .AppendLine($"    \"{nameof(content)}\"  : \"{
                    new StringBuilder(content)
                        .Replace("\r\n", "\\n")
                        .Replace("\n", "\\n")
                        .Replace("\"", "\\\"")
                }\" ,")
                .AppendLine("},")
                .AppendLine("\n");
            return sb.ToString();
        }
    }
}

internal class AntelcatLogger<TCategoryName> : AntelcatLogger, IAntelcatLogger<TCategoryName>
{
    public AntelcatLogger(IAntelcatLoggerFactory factory) : base(factory, typeof(TCategoryName).Name)
    {
    }
}