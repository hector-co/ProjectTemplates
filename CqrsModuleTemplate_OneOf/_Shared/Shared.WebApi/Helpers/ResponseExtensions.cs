using OneOf;
using Shared.Domain.Responses;
using Shared.WebApi.ExceptionHandling;
using System.Net;

namespace Shared.WebApi.Helpers;

public static class ResponseExtensions
{
    public static void Verify<T0>(this OneOf<T0, FailedResponse> response)
    {
        response.Switch(
            success => { },
            failure =>
            {
                var statusCode = HttpStatusCode.UnprocessableEntity;
                var code = WebApiException.DomainException;
                if (failure.HasErrors && failure.Errors[0].Code.EndsWith(".NotFound"))
                {
                    statusCode = HttpStatusCode.NotFound;
                    code = WebApiException.DataAccessError;
                }

                throw new WebApiException("Error while executing the request.", statusCode, code, failure,
                    failure?.InnerException);
            }
        );
    }
}