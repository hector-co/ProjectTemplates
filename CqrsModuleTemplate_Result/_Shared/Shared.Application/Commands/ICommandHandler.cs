using LanguageExt.Common;
using MediatR;

namespace Shared.Application.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result<bool>>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TValue> : IRequestHandler<TCommand, Result<TValue>>
    where TCommand : ICommand<TValue>
{
}
