using LanguageExt.Common;
using QueryX.Exceptions;
using Shared.Domain;
using Shared.WebApi.ExceptionHandling;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Shared.WebApi.Helpers;

public static class ResponseExtensions
{
    private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static IResult ToIResult<TValue>(this Result<TValue> result, HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
    {
        return result.ToIResult(_ => TypedResults.NoContent(), errorStatusCode);
    }

    public static IResult ToIResult<TValue>(this Result<TValue> result, Func<TValue, IResult> success, HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
    {
        return result.Match(
            value => success(value),
            exception =>
            {
                var errorModel = GetErrorModel(exception, errorStatusCode);

                var result = JsonSerializer.Serialize(errorModel, jsonSerializerOptions);
                return TypedResults.Content(result, "application/json", statusCode: (int)errorStatusCode);
            });
    }

    public static async Task<IResult> ToIResult<TValue>(this Result<TValue> result, Func<TValue, Task<IResult>> success, HttpStatusCode errorStatusCode = HttpStatusCode.UnprocessableEntity)
    {
        return await result.Match<Task<IResult>>(
            async value => await success(value),
            exception =>
            {
                var errorModel = GetErrorModel(exception, errorStatusCode);

                var result = JsonSerializer.Serialize(errorModel, jsonSerializerOptions);
                return Task.FromResult<IResult>(TypedResults.Content(result, "application/json", statusCode: (int)(errorModel.Status ?? 0)));
            });
    }

    private static ErrorResultModel GetErrorModel(Exception exception, HttpStatusCode errorStatusCode)
    {
        var errorModel = ExToErrorModel(exception, errorStatusCode);

        var innerErrors = new List<ErrorResultModel>();
        var currEx = exception.InnerException;
        while (currEx != null)
        {
            innerErrors.Add(ExToErrorModel(currEx, default));
            currEx = currEx.InnerException;
        }

        errorModel.Payload = innerErrors.Count > 0 ? innerErrors : null;

        return errorModel;
    }

    private static ErrorResultModel ExToErrorModel(Exception exception, HttpStatusCode? errorStatusCode)
    {
        var errorResult = new ErrorResultModel("Error while processing request.", errorStatusCode,
             WebApiException.InternalError);

        switch (exception)
        {
            case DomainException dex:
                errorResult.Code = dex.Code;
                errorResult.Message = dex.Message;
                if (dex.Code.EndsWith(".NotFound"))
                {
                    if (errorStatusCode != default)
                        errorResult.Status = HttpStatusCode.NotFound;
                }
                else
                {
                    if (errorStatusCode != default)
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
            default:
                errorResult.Message = "An unexpected error occurred.";
                errorResult.Status = HttpStatusCode.InternalServerError;
                break;
        }

        return errorResult;
    }
}