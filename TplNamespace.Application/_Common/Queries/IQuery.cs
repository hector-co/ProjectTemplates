using MediatR;

namespace TplNamespace.Application.Common.Queries;

public interface IQuery<TData> : IRequest<Result<TData>>
{
}

