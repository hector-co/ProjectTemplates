using MediatR;

namespace Shared.Application.Queries;

public interface IQueryHandler<in TQuery, TData> : IRequestHandler<TQuery, Result<TData>>
    where TQuery : IQuery<TData>
{
}

