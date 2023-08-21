using System.Text.Json.Serialization;
using Antelcat.Core.Extensions;
using Antelcat.Extensions;

namespace Antelcat.Core.Models;

public class Response
{
    [JsonPropertyOrder(1)]
    public int Code { get; set; } = 1;

#if msg
        [JsonPropertyName("msg")]
#endif
    [JsonPropertyOrder(3)]
    public string Message { get; set; } = string.Empty;
    [JsonPropertyOrder(4)]
    public long Timestamp { get; set; } = TimeExtension.CurrentTimestamp();

    public static implicit operator Response(int code) => new() { Code = code };
    public static implicit operator Response(string message) => new() { Code = 0, Message = message };
    public static implicit operator Response(Exception exception) => new() { Code = 0, Message = exception.Message };
    public static implicit operator Response((int,string) tuple) => new() { Code = tuple.Item1, Message = tuple.Item2 };
    public override string ToString() => this.Serialize();
}

public class Response<T> : Response
{
    [JsonPropertyOrder(2)]
    public T Data { get; set; } = default!;

    public Response() { }
    public Response(T data) { Data = data; }

    public static implicit operator Response<T>(T data) => new(data);
    public static implicit operator Response<T>((T, string) data) => new(data.Item1) { Code = 0, Message = data.Item2 };
    public static implicit operator Response<T>((int, T, string) data) => new(data.Item2) { Code = data.Item1, Message = data.Item3 };
    public static implicit operator Response<T>(int code) => new() { Code = code };
    public static implicit operator Response<T>(string message) => new() { Code = 0, Message = message };
    public static implicit operator Response<T>(Exception exception) => new() { Code = 0, Message = exception.Message };
}