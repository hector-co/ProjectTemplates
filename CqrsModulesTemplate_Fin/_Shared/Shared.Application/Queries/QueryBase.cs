namespace Shared.Application.Queries;

public record QueryBase<TData> : RequestBase<QueryResult<TData>>, IQuery<TData>;
