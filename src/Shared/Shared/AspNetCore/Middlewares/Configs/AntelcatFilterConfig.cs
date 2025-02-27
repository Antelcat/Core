using System.Net.Mime;
using Antelcat.Server.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Antelcat.Server.Configs;

public class AntelcatFilterConfig
{
    /// <summary>
    /// Determine whether output <see cref="Exception"/>.<see cref="Exception.ToString"/> default is <remarks>true</remarks>
    /// </summary>
    public bool Strict
    {
        set
        {
            if (value) writeDefault = (_, _) => Task.CompletedTask;
            else writeDefault       = (e, ctx) => ctx.WriteAsync(e.ToString());
        }
    }

    public AntelcatFilterConfig RegisterExceptionHandler<TException>(Func<TException, HttpResponse, Task> handler)
        where TException : Exception
    {
        exceptionHandlers[typeof(TException)] = (exception, response) => handler((TException)exception, response);
        return this;
    }

    public AntelcatFilterConfig RegisterExceptionHandler(Type exceptionType,
                                                         Func<Exception, HttpResponse, Task> handler)
    {
        exceptionHandlers[exceptionType] = (exception, response) => handler((Exception)exception, response);
        return this;
    }

    internal Task ExecuteHandler(Exception exception, HttpResponse response) =>
        exceptionHandlers.TryGetValue(exception.GetType(), out var handler)
            ? handler.Invoke(exception, response)
            : writeDefault(exception, response);

    private Func<Exception, HttpResponse, Task> writeDefault = (_, _) => Task.CompletedTask;

    private readonly Dictionary<Type, Func<object, HttpResponse, Task>> exceptionHandlers = new();

    internal static readonly AntelcatFilterConfig Default = new()
    {
        exceptionHandlers =
        {
            {
                typeof(RejectException), static (exception, response) =>
                {
                    var reject = (exception as RejectException)!;
                    response.StatusCode = reject.StatusCode;
                    switch (reject.Data)
                    {
                        case null:
                            response.ContentType = MediaTypeNames.Text.Plain;
                            return response.WriteAsync(string.Empty);
                        case string content:
                            return response.WriteAsync(content);
                        case not null when reject.Data.GetType().IsPrimitive:
                            response.ContentType = MediaTypeNames.Text.Plain;
                            return response.WriteAsync($"{reject.Data}");
                        default:
                            response.ContentType = MediaTypeNames.Application.Json;
                            return response.WriteAsJsonAsync(reject.Data);
                    }
                }
            }
        }
    };
}