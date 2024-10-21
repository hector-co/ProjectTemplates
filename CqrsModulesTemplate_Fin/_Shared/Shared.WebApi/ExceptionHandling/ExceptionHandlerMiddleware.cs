using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using QueryX.Exceptions;
using Shared.Domain;

namespace Shared.WebApi.ExceptionHandling;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly ILogger _logger = loggerFactory.CreateLogger("ExceptionHandler");

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the error handler will not be executed.");
                throw;
            }
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        var errorResult = new ErrorResultModel("Error while processing request.", HttpStatusCode.BadRequest,
            WebApiException.InternalError);

        switch (exception)
        {
            case DomainException dex:
                errorResult.Code = dex.Code;
                errorResult.Message = dex.Message;
                if (dex.Code.EndsWith(".NotFound"))
                {
                    errorResult.Status = HttpStatusCode.NotFound;
                }
                if (dex.Code.EndsWith(".Unauthorized"))
                {
                    errorResult.Status = HttpStatusCode.Unauthorized;
                }
                else
                {
                    errorResult.Status = HttpStatusCode.UnprocessableEntity;
                }
                break;
            case QueryException:
                errorResult.Code = WebApiException.ParametersFormat;
                break;
            case ValidationException vex:
                errorResult.Code = WebApiException.ValidationError;
                errorResult.Payload = vex.Errors;
                break;
            case WebApiException apiEx:
                errorResult.Message = exception.Message;
                errorResult.Status = apiEx.Status;
                errorResult.Code = apiEx.Code;
                errorResult.Payload = apiEx.Payload;
                break;
            case BadHttpRequestException:
                errorResult.Message = "Error al procesar la solicitud.";
                errorResult.Status = HttpStatusCode.BadRequest;
                errorResult.Code = WebApiException.ParametersFormat;
                break;
            default:
                errorResult.Message = "Ocurrió un error inesperado.";
                errorResult.Status = HttpStatusCode.InternalServerError;
                break;
        }

        logger.LogError(exception, "Exception was thrown. Url: {url}", context.Request.GetDisplayUrl());

        var result = JsonSerializer.Serialize(errorResult, Options);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)(errorResult?.Status ?? HttpStatusCode.InternalServerError);
        return context.Response.WriteAsync(result);
    }
}