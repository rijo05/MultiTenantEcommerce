using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.GeneratePaymentSession;
public record GeneratePaymentSessionCommand(
    Guid OrderId, 
    Guid CustomerId) : ICommand<PaymentResultDTO>;
