using Microsoft.AspNetCore.Http;

namespace Shared.WebApi.Auth;

public class SessionInfoEndpointFilter : IEndpointFilter
{
    private readonly SessionInfo _sessionInfo;

    public SessionInfoEndpointFilter(SessionInfo sessionInfo)
    {
        _sessionInfo = sessionInfo;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
            await _sessionInfo.SetData(context.HttpContext);

        return await next(context);
    }
}
