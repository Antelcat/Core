using System.Reflection;
using Antelcat.Core.Interface.IL;
using Antelcat.Core.Structs.IL;
using Antelcat.Core.Extensions;

namespace Antelcat.Core.Implements.Loggers;

public class LoggerConfig
{
    #region Configs

    protected bool OutputConsole { get; set; } = true;
    protected string Directory { get; set; } = Path.Combine(Environment.CurrentDirectory, "Logs");
    protected Func<DateTime, string> NamingFormat { get; set; } = time => $"{nameof(Antelcat)}[{time:yyyy-MM-dd}].log";
    protected string Prefix { get; set; } = "/**";
    protected string Suffix { get; set; } = "*/";

    #endregion

    public LoggerConfig WithDirectory(string directory)
    {
        Directory = directory;
        return this;
    }

    public LoggerConfig WithPrefix(string prefix)
    {
        Prefix = prefix;
        return this;
    }

    public LoggerConfig WithSuffix(string suffix)
    {
        Suffix = suffix;
        return this;
    }

    public LoggerConfig WithFileNameFormat(Func<DateTime, string> namingFormat)
    {
        NamingFormat = namingFormat;
        return this;
    }

    public LoggerConfig OutputToConsole(bool output)
    {
        OutputConsole = output;
        return this;
    }

    #region Reflects

    private static readonly IEnumerable<Tuple<ILMethod, ILMethod>> Callers = typeof(LoggerConfig)
        .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(x => x is { CanRead: true, CanWrite: true })
        .Select(x => new Tuple<ILMethod, ILMethod>((ILSetter)x, (ILGetter)x));

    internal virtual void Initialize<TCategory>(AntelcatLogger<TCategory> logger) =>
        Callers
            .ForEach(p => 
                p.Item1.Invoke(logger, p.Item2.Invoke(this)));

    #endregion
}