using System.Net;

namespace Shared.WebApi.ExceptionHandling;

public class WebApiException(string message, HttpStatusCode statusCode, string code, object? payload = null, Exception? innerException = null) : Exception(message, innerException)
{
    public const string InternalError = "internal_error";
    public const string ValidationError = "validation_error";
    public const string ParametersFormat = "parameters_format_error";
    public const string DomainException = "domain_error";
    public const string DataAccessError = "data_access_error";

    public HttpStatusCode Status { get; } = statusCode;
    public string Code { get; } = code;
    public object? Payload { get; } = payload;
}