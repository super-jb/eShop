using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Models;

namespace Ordering.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : JsonApiResponse, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    // private readonly IValidator<TRequest> _validationHandler;

    // Have 2 constructors in case the validator does not exist
    public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    //public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger, IValidator<TRequest> validationHandler)
    //    : this(logger)
    //{
    //    _validationHandler = validationHandler;
    //}

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    : this(logger)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            ValidationContext<TRequest> context = new (request);

            FluentValidation.Results.ValidationResult[] validationResults = 
                await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            List<FluentValidation.Results.ValidationFailure> failures = 
                validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                throw new Exceptions.ValidationException(failures);
            }
        }

        Type req = request.GetType();
        _logger.LogInformation($"Validation successful for {req.Name}.");
        return await next();
    }
}
