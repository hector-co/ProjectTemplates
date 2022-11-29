using MediatR;

namespace TplNamespace.Application.Abstractions.Queries;

public interface IQueryHandler<in TQuery, TData> : IRequestHandler<TQuery, Result<TData>>
    where TQuery : IQuery<TData>
{
}

