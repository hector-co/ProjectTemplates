using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Shared.Application.Behaviors;

public class LoggerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IOneOf
{
    private readonly ILogger _logger;

    public LoggerPipelineBehavior(ILogger<LoggerPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        var result = await next();

        if (result.Index > 0)
        {
            _logger.LogWarning("Request failure {@RequestName}, {@Error}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                result.Value,
                DateTime.UtcNow);
        }

        _logger.LogInformation("Completed request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        return result;
    }
}
