using System.Text.Json.Serialization;
using Feast.Foundation.Core.Extensions;

namespace Feast.Foundation.Core.Models
{
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
        public long Timestamp { get; set; } = TimeExtension.CurrentTimestamp;

        public static implicit operator Response(int code) => new() { Code = code };
        public static implicit operator Response(string message) => new() { Code = 0, Message = message };
        public static implicit operator Response(Exception exception) => new() { Code = 0, Message = exception.Message };
    }

    public class Response<T> : Response
    {
        [JsonPropertyOrder(2)]
        public T Data { get; set; } = default!;

        public Response() { }
        public Response(T data) { Data = data; }

        public static implicit operator Response<T>(T data) => new(data);
        public static implicit operator Response<T>(int code) => new() { Code = code };
        public static implicit operator Response<T>(string message) => new() { Code = 0, Message = message };
        public static implicit operator Response<T>(Exception exception) => new() { Code = 0, Message = exception.Message };
    }
}
