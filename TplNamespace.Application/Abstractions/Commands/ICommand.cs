using MediatR;
using TplNamespace.Domain.Abstractions;

namespace TplNamespace.Application.Abstractions.Commands;

public interface ICommand : IRequest<Response>
{
}

public interface ICommand<TValue> : IRequest<Response<TValue>>

{
}
