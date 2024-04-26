using LanguageExt.Common;
using Shared.Domain;
using Shared.WebApi.ExceptionHandling;
using System.Net;

namespace Shared.WebApi.Helpers;

public static class ResponseExtensions
{
    public static void Verify<TValue>(this Result<TValue> result, HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
    {
        _ = result.IfFail(
            error =>
            {
                ProcessError(error, statusCode);
            });
    }

    public static TResponse IfSuccess<TValue, TResponse>(this Result<TValue> result, Func<TValue, TResponse> success, HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
    {
        return result.Match(
            Succ: value => success(value),
            Fail: error =>
            {
                ProcessError(error, statusCode);
                return default;
            });
    }

    private static void ProcessError(Exception error, HttpStatusCode statusCode)
    {
        var code = WebApiException.DomainException;
        if (error is DomainException dEx && dEx.Code.EndsWith(".NotFound"))
        {
            statusCode = HttpStatusCode.NotFound;
            code = WebApiException.DataAccessError;
        }
        throw new WebApiException("Error while executing the request.", statusCode, code, error, error.InnerException);
    }
}