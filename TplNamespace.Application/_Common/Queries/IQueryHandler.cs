using MediatR;

namespace TplNamespace.Application.Common.Queries;

public interface IQueryHandler<in TQuery, TData> : IRequestHandler<TQuery, Result<TData>>
    where TQuery : IQuery<TData>
{
}

