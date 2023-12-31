using System.Text.Json.Serialization;

namespace Shared.Domain.Responses;

public record SuccessResponse
{
    public SuccessResponse()
    {
        Warnings = [];
    }

    public SuccessResponse(params ResponseMessage[] warnings)
    {
        Warnings = warnings;
    }

    public ResponseMessage[] Warnings { get; }

    [JsonIgnore]
    public bool HasWarnings => Warnings.Length > 0;
}
