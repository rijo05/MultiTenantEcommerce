using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.AddPrice;
public record AddSubscriptionPlanPriceCommand(
    string StripeProductId,
    string StripePriceId,
    long Amount) : ICommand<Unit>;
