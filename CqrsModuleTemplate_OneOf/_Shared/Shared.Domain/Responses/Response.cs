namespace Shared.Domain.Responses;

public class Response
{
    public static SuccessResponse Success(params ResponseMessage[] warnings)
    {
        return new SuccessResponse(warnings);
    }

    public static SuccessResponse<TValue> Success<TValue>(TValue value, params ResponseMessage[] warnings)
    {
        return new SuccessResponse<TValue>(value, warnings);
    }

    public static FailedResponse Failure(params ResponseMessage[] errors)
    {
        return new FailedResponse(errors);
    }

    public static FailedResponse Failure(Exception exception, params ResponseMessage[] errors)
    {
        return new FailedResponse(exception, errors);
    }
}
