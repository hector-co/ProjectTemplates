using LanguageExt.Common;
using MediatR;

namespace Shared.Application.Commands;

public interface ICommand<TValue> : IRequest<Result<TValue>>
{
}

public interface ICommand : ICommand<bool>
{
}

