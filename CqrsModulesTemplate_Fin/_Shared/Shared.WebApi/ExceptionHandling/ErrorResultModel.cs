using System.Net;

namespace Shared.WebApi.ExceptionHandling;

public class ErrorResultModel(string message, HttpStatusCode? status, string code, object? payload = null)
{
    public string Message { get; set; } = message;

    public HttpStatusCode? Status { get; set; } = status;

    public string Code { get; set; } = code;

    public object? Payload { get; set; } = payload;
}