using MediatR;
using Shared.Application.Auth;

namespace Shared.Application.Behaviors;

public class RequestSessionInfoBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : RequestBase<TResponse>
{
    private readonly ISessionInfo _sessionInfo;

    public RequestSessionInfoBehavior(ISessionInfo sessionInfo)
    {
        _sessionInfo = sessionInfo;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.SessionInfo = _sessionInfo;
        return next();
    }
}
