using System.Reflection;
using Feast.Foundation.Core.Extensions;

namespace Feast.Foundation.Core.Implements.Loggers;

public class LoggerConfig
{
    #region Configs

    protected bool OutputConsole { get; set; } = true;
    protected string Directory { get; set; } = Path.Combine(Environment.CurrentDirectory, "Logs");
    protected Func<DateTime, string> NamingFormat { get; set; } = time => $"{nameof(Feast)}[{time:yyyy-MM-dd}].log";
    protected string Prefix { get; set; } = "/**";
    protected string Suffix { get; set; } = "*/";
    #endregion

    public LoggerConfig WithDirectory(string directory) { Directory = directory; return this; }
    public LoggerConfig WithPrefix(string prefix) { Prefix = prefix; return this; }
    public LoggerConfig WithSuffix(string suffix) { Suffix = suffix; return this; }
    public LoggerConfig WithFileNameFormat(Func<DateTime, string> namingFormat) { NamingFormat = namingFormat; return this; }
    public LoggerConfig OutputToConsole(bool output) { OutputConsole = output; return this; }

    #region Reflects
    private readonly PropertyInfo[] properties = typeof(LoggerConfig)
        .GetProperties(
            BindingFlags.NonPublic
            | BindingFlags.GetProperty
            | BindingFlags.SetProperty
            | BindingFlags.Instance);

    internal virtual void Initialize<TCategory>(FeastLogger<TCategory> logger)
    {
        properties
            .Where(x => x is { CanRead: true, CanWrite: true })
            .ForEach(p => p.SetValue(logger, p.GetValue(this)));
    }
    #endregion
}