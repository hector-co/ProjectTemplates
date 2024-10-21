using MediatR;

namespace Shared.Application.Queries;

public interface IQueryHandler<in TQuery, TData> : IRequestHandler<TQuery, QueryResult<TData>>
    where TQuery : IQuery<TData>
{
}

