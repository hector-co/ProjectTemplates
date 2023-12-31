using System.Text.Json.Serialization;

namespace Shared.Domain.Responses;

public record FailedResponse
{
    public FailedResponse(params ResponseMessage[] errors)
    {
        InnerException = default;
        Errors = errors;
    }

    public FailedResponse(Exception exception, params ResponseMessage[] errors)
    {
        InnerException = exception;
        Errors = errors;
    }

    public ResponseMessage[] Errors { get; }

    [JsonIgnore]
    public bool HasErrors => Errors.Length > 0;

    public Exception? InnerException { get; } = default;
}
