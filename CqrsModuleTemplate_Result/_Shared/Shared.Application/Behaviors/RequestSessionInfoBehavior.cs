using MediatR;
using Shared.Application.Auth;

namespace Shared.Application.Behaviors;

public class RequestSessionInfoBehavior<TRequest, TResponse>(ISessionInfo sessionInfo) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : RequestBase<TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.SessionInfo = sessionInfo;
        return next();
    }
}
