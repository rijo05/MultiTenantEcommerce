using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.DeletePlan;
public record DeactivateSubscriptionPlanCommand(
    string StripeProductId) : ICommand<Unit>;
