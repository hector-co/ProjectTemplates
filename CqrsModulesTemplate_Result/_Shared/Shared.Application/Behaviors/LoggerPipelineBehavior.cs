using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Application.Behaviors;

public class LoggerPipelineBehavior<TRequest, TResponse>(ILogger<LoggerPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request starting {@RequestName}", typeof(TRequest).Name);

        var result = await next();

        logger.LogInformation("Request completed {@RequestName}", typeof(TRequest).Name);

        return result;
    }
}
