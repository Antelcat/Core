using System.Diagnostics;
using System.Text;
using System.Threading.Channels;
using Antelcat.Core.Interface.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Antelcat.Core.Implements.Loggers;

internal class AntelcatLogger : LoggerConfig, IAntelcatLogger
{
    /// <summary>
    /// static，因为多个logger可能输出到一个文件
    /// </summary>
    private static readonly Channel<(string LogFilePath, string LogContent)> LogChannel = 
        Channel.CreateUnbounded<(string LogFilePath, string LogContent)>();
    
    private static async Task OutputToFileTask()
    {
        while (await LogChannel.Reader.WaitToReadAsync())
        {
            while (LogChannel.Reader.TryRead(out var tuple))
            {
                await File.AppendAllTextAsync(tuple.LogFilePath, tuple.LogContent);
            }
        }
    }
    
    static AntelcatLogger()
    {
        Task.Factory.StartNew(OutputToFileTask, TaskCreationOptions.LongRunning);
    }
    
    public AntelcatLogger(IAntelcatLoggerFactory factory, string category)
    {
        Category = category;
        (factory as AntelcatLoggerFactory)!.Initialize(this);
        OnLog += OutputToFile;
    }

    public AntelcatLogger(IAntelcatLoggerFactory factory) : this(factory,
        GetCaller() ?? string.Empty) { }

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
    
    protected override string LogFolderPath
    {
        get
        {
            Directory.CreateDirectory(base.LogFolderPath);
            return base.LogFolderPath;
        }
    }

    private string LogFilePath
    {
        get
        {
            var logFilePath = Path.Combine(LogFolderPath, NamingFormat(DateTime.Now));
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }

            return logFilePath;
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

    private static StackFrame? GetCallerFrame(int deepness) =>
        new StackTrace(deepness, true).GetFrames().FirstOrDefault(x =>
        {
            var method = x.GetMethod();
            if (method == null || method.Module.FullyQualifiedName.StartsWith("Microsoft.Extensions.Logging")) return false;
            return method.Name.StartsWith("Log") is false;
        });

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

    private async Task OutputToFile(string log)
    {
        await LogChannel.Writer.WriteAsync((LogFilePath, log));
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel)) LogInternal(GetCallerFrame(2), logLevel, eventId, formatter(state, exception));
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

internal class AntelcatLogger<TCategoryName>(IAntelcatLoggerFactory factory)
    : AntelcatLogger(factory, typeof(TCategoryName).Name), IAntelcatLogger<TCategoryName>;