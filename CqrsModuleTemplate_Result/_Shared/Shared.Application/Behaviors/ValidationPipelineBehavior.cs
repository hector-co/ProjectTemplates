using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger _logger;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults =
                await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Any())
            {
                _logger.LogInformation("Request validation failed {@RequestName}, {@Errors}, {@DateTimeUtc}",
                    typeof(TRequest).Name,
                    failures,
                    DateTime.UtcNow);

                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}
