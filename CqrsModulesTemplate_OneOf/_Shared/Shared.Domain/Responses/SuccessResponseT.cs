namespace Shared.Domain.Responses;

public record SuccessResponse<TValue> : SuccessResponse
{
    public SuccessResponse(TValue value)
    {
        Value = value;
    }

    public SuccessResponse(TValue value, params ResponseMessage[] warnings) : base(warnings)
    {
        Value = value;
    }

    public TValue Value { get; }
}
