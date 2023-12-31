using MediatR;
using OneOf;
using Shared.Domain.Responses;

namespace Shared.Application.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, OneOf<SuccessResponse, FailedResponse>>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TValue> : IRequestHandler<TCommand, OneOf<SuccessResponse<TValue>, FailedResponse>>
    where TCommand : ICommand<TValue>
{
}