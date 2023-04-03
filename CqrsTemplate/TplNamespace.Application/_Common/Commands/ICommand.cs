using MediatR;
using TplNamespace.Domain.Common;

namespace TplNamespace.Application.Common.Commands;

public interface ICommand : IRequest<Response>
{
}

public interface ICommand<TValue> : IRequest<Response<TValue>>

{
}
