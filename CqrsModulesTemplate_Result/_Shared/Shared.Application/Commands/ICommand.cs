using MediatR;

namespace Shared.Application.Commands;

public interface ICommand<TValue> : IRequest<TValue>
{
}
