﻿using System.Diagnostics;
using System.Text;
using Antelcat.Core.Enums;
using Antelcat.Core.Interface.Logging;

namespace Antelcat.Core.Implements.Loggers;

internal abstract class AntelcatLogger : LoggerConfig, IAntelcatLogger
{
    #region Fields
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
            if (!File.Exists(log)) { File.Create(log).Close(); }
            return log;
        }
    }
    protected abstract string Category { get; }
    #endregion

    #region Methods
    private string Format(string content) => $"{Prefix} {content} {Suffix}";
    #endregion

    public void Log<TState>(
        LogLevel level,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        var sb = new StringBuilder();
        var ps = new StackTrace(1, true)
            .GetFrames()!
            .FirstOrDefault(x => x.GetMethod()?.Module.Assembly != typeof(IAntelcatLogger).Assembly);
        var method = ps?.GetMethod();
        sb
            .AppendLine(Format($"等级 : [ {level} ], 时间: [ {DateTime.Now} ]"))
            .AppendLine(Format($"模块 : [ {method?.Module.Assembly.GetName()} ]"))
            .AppendLine(Format($"类型 : [ {Category} ]"));
        if (ps != null)
        {
            sb
                .AppendLine(Format($@"文件 : [ ""{ps.GetFileName()}"" ], 行: {ps.GetFileLineNumber()}"))
                .AppendLine(Format($"方法 : [ {method} ]"));
        }
        sb
            .AppendLine(formatter(state, exception))
            .AppendLine("\n");
        var log = sb.ToString();
        File.AppendAllText(LogFile, log);
        if (OutputConsole) { Console.WriteLine(log); }
    }
}

internal class AntelcatLogger<TCategoryName> : AntelcatLogger, IAntelcatLogger<TCategoryName>
{
    public AntelcatLogger(LoggerFactory factory)
    {
        factory.Initialize(this);
    }

    protected override string Category { get; } = typeof(TCategoryName).Name;
}