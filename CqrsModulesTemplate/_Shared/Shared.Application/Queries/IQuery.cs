using MediatR;

namespace Shared.Application.Queries;

public interface IQuery<TData> : IRequest<QueryResult<TData>>
{
}
