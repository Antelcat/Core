using System.Text.Json.Serialization;
using Feast.Foundation.Extensions;


namespace Feast.Foundation.Models
{
    public class HttpResponse
    {
        public int Code { get; set; } = 1;

#if msg
        [JsonPropertyName("msg")]
#endif
        public string Message { get; set; } = string.Empty;
        public long Timestamp { get; set; } = TimeExtension.CurrentTimestamp;

        public static implicit operator HttpResponse(int code) => new() { Code = code };
        public static implicit operator HttpResponse(string message) => new() { Code = 0, Message = message };
        public static implicit operator HttpResponse(Exception exception) => new() { Code = 0, Message = exception.Message };
    }

    public class HttpResponse<T> : HttpResponse
    {
        public T Data { get; set; } = default!;

        public static implicit operator HttpResponse<T>(T data) => new() { Data = data };
        public static implicit operator HttpResponse<T>(int code) => new() { Code = code };
        public static implicit operator HttpResponse<T>(string message) => new() { Code = 0, Message = message };
        public static implicit operator HttpResponse<T>(Exception exception) => new() { Code = 0, Message = exception.Message };
    }
}
