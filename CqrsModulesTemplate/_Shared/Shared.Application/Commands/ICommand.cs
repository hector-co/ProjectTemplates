using MediatR;
using Shared.Domain;

namespace Shared.Application.Commands;

public interface ICommand : IRequest<Response>
{
}

public interface ICommand<TValue> : IRequest<Response<TValue>>

{
}
