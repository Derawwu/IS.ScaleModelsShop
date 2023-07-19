using FluentValidation;
using MediatR;

namespace IS.ScaleModelsShop.Application.Middleware;

/// <summary>
/// Validation behaviour.
/// </summary>
/// <typeparam name="TRequest">Type of input data.</typeparam>
/// <typeparam name="TResponse">Type of response data.</typeparam>
public class ValidationForEmptyResponseBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    /// <summary>
    /// Initializes a new instance of <see cref="ValidationForEmptyResponseBehaviour{TRequest, TResponse}"/>
    /// </summary>
    /// <param name="validators"><see cref="IEnumerable{IValidator{TRequest}}"/>.</param>
    public ValidationForEmptyResponseBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _ = next ?? throw new ArgumentNullException(nameof(next));

        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults =
                await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var errors = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (errors.Any()) throw new ValidationException(errors);
        }

        return await next();
    }
}