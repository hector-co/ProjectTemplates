using MediatR;

namespace TplNamespace.Application.Abstractions.Queries;

public interface IQuery<TData> : IRequest<Result<TData>>
{
}

