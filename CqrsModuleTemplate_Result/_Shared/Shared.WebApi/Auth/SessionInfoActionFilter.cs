using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.WebApi.Auth;

public class SessionInfoActionFilter(SessionInfo sessionInfo) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
            await sessionInfo.SetData(context.HttpContext);

        await next();
    }
}
