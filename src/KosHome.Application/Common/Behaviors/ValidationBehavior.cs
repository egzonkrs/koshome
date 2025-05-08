using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using FluentValidation;
using KosHome.Domain.Common;
using MediatR;

namespace KosHome.Application.Common.Behaviors;

/// <summary>
/// Validates a request with all registered <see cref="IValidator{T}"/>s
/// and returns a failed <see cref="Result{T}"/> when any rule breaks.
/// </summary>
/// <typeparam name="TRequest">Concrete request type.</typeparam>
/// <typeparam name="TResponse">Inner payload carried by <see cref="Result{T}"/>.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : ResultBase
{
    // IRequest<Result<Ulid>>
    // Result<Ulid>
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task
            .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToArray();

        if (failures.Length is 0)
        {
            return await next();
        }

        var errors = failures
            .Select(f => new CustomFluentError("VALIDATION_ERROR", f.ErrorMessage))
            .ToList();

        return Result.Fail(errors) as TResponse;
    }
}