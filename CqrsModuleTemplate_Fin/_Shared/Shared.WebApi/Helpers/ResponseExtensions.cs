using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using LanguageExt;
using Shared.Domain;

namespace Shared.WebApi.Helpers;

public static class ResponseExtensions
{
    private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static IResult ToIResult<TValue>(this Fin<TValue> result, HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
    {
        return result.Match<IResult>(
            _ => TypedResults.NoContent(),
            error =>
            {
                if (error.Code == DomainErrors.Common.NotFound)
                {
                    errorStatusCode = HttpStatusCode.NotFound;
                }
                var result = JsonSerializer.Serialize(error, jsonSerializerOptions);
                return TypedResults.Content(result, "application/json", statusCode: (int)errorStatusCode);
            });
    }

    public static async Task<IResult> ToIResult<TValue>(this Fin<TValue> result, Func<TValue, Task<IResult>> success, HttpStatusCode errorStatusCode = HttpStatusCode.BadRequest)
    {
        return await result.Match<Task<IResult>>(
            async value => await success(value),
            error =>
            {
                if (error.Code == DomainErrors.Common.NotFound)
                {
                    errorStatusCode = HttpStatusCode.NotFound;
                }
                var result = JsonSerializer.Serialize(error, jsonSerializerOptions);
                return Task.FromResult(TypedResults.Content(result, "application/json", statusCode: (int)errorStatusCode) as IResult);
            });
    }
}