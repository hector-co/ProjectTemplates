using Microsoft.AspNetCore.Http;

namespace Shared.WebApi.Auth;

public class SessionInfoEndpointFilter(SessionInfo sessionInfo) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
            await sessionInfo.SetData(context.HttpContext);

        return await next(context);
    }
}
