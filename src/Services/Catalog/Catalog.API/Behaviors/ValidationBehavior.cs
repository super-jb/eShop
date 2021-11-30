using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : JsonApiResponse, new()
    {
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
        private readonly IValidator<TRequest> _validationHandler;

        // Have 2 constructors in case the validator does not exist
        public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger, IValidator<TRequest> validationHandler)
            : this(logger)
        {
            _validationHandler = validationHandler;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Type requestName = request.GetType();
            if (_validationHandler == null)
            {
                _logger.LogInformation($"{requestName} does not have a validation handler configured.");
                return await next();
            }

            ValidationResult result = await _validationHandler.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage);
                _logger.LogWarning($"Validation failed for {requestName}. Errors: {string.Join("; ", errorMessages)}");
                return new TResponse 
                { 
                    StatusCode = HttpStatusCode.BadRequest, 
                    ErrorMessages = errorMessages
                };
            }

            _logger.LogInformation($"Validation successful for {requestName}.");
            return await next();
        }
    }
}

