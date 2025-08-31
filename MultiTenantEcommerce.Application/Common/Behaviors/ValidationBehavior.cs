using FluentValidation;
using MediatR;
using MultiTenantEcommerce.Application.Common.Validators;

namespace MultiTenantEcommerce.Application.Common.Behaviors;
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await ValidationRules.ValidateAsync(request, _validator);

        return await next();
    }
}
