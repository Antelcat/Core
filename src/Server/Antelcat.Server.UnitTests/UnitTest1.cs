using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net;
using System.Text;
using Antelcat.Enums;
using Antelcat.Extensions;
using Antelcat.Server.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Antelcat.Server.UnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
    
    [Test]
    public void TestException()
    {
        var str = JsonSerializer.Serialize("{ A : 1}");
        Debugger.Break();
        try
        {
            throw (RejectException)(HttpStatusCode.InternalServerError, null);
        }
        catch (RejectException ex)
        {
            var r = ex.ToString();
            Debugger.Break();
        }
    }

    [Test]
    public void TestLogResult()
    {
        var path =
            @"D:\Shared\WorkSpace\Git\EasyPathology.Server\extern\Server\src\Antelcat.Server.Test\bin\Debug\net8.0\Logs\Antelcat[2023-11-22].log";

        var txt     = File.ReadAllText(path);
        var logInfos = $"[{txt}]".Deserialize<List<LogInfo>>(SerializeOptions.Default | SerializeOptions.EnumToString);
        Debugger.Break();
    }

    public class LogInfo
    {
        public LogLevel LogLevel { get; set; }
        public string?   Category { get; set; }
        public string?   Method   { get; set; }
        public string?   Module   { get; set; }
        public string?   Time     { get; set; }
        public string?   File     { get; set; }
        public string?   Content  { get; set; }
    }
}