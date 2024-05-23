using MediatR;
using Shared.Domain;

namespace Shared.Application.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Response>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TValue> : IRequestHandler<TCommand, Response<TValue>>
    where TCommand : ICommand<TValue>
{
}