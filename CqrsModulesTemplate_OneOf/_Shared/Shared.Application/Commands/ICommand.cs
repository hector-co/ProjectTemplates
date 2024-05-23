using MediatR;
using OneOf;
using Shared.Domain.Responses;

namespace Shared.Application.Commands;

public interface ICommand : IRequest<OneOf<SuccessResponse, FailedResponse>>
{
}

public interface ICommand<TValue> : IRequest<OneOf<SuccessResponse<TValue>, FailedResponse>>
{
}
