using MediatR;

namespace Shared.Application.Commands;

public interface ICommandHandler<in TCommand, TValue> : IRequestHandler<TCommand, TValue>
    where TCommand : ICommand<TValue>
{
}
