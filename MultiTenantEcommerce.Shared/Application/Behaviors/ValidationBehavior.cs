using FluentValidation;
using MediatR;

namespace MultiTenantEcommerce.Shared.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        await ValidateAsync(request, _validator);

        return await next();
    }

    public static async Task ValidateAsync<T>(T entity, IValidator<T> validator)
    {
        var validationResults = await validator.ValidateAsync(entity);

        if (!validationResults.IsValid)
            throw new ValidationException(validationResults.Errors);
    }
}