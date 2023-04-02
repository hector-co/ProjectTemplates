using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http.Extensions;
using QueryX.Exceptions;

namespace TplNamespace.WebApi.ExceptionHandling;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
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
            case QueryException:
                errorResult.Code = WebApiException.ParametersFormat;
                logger.LogInformation(exception, "QueryException: {@essage}. Url: {@url}", exception.Message, context.Request.GetDisplayUrl());
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
            default:
                errorResult.Message = "An unexpected error occurred.";
                errorResult.Status = HttpStatusCode.InternalServerError;
                logger.LogError(exception, "Exception was thrown: {@essage}. Url: {@url}", errorResult, context.Request.GetDisplayUrl());
                break;
        }


        var result = JsonSerializer.Serialize(errorResult);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)errorResult.Status;
        return context.Response.WriteAsync(result);
    }
}