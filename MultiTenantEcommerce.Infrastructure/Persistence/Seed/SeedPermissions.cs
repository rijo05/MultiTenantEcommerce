using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Seed;

public static class SeedPermissions
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!context.Permissions.Any())
        {
            var permissions = new List<Permission>
            {
                //CATEGORY
                new("read.category", "Ver categoria", "category", "read"),
                new("create.category", "Criar categoria", "category", "create"),
                new("update.category", "Atualizar categoria", "category", "update"),
                new("delete.category", "Apagar categoria", "category", "delete"),

                //PRODUCT
                new("read.product", "Ver produto", "product", "read"),
                new("create.product", "Criar produto", "product", "create"),
                new("update.product", "Atualizar produto", "product", "update"),
                new("delete.product", "Apagar produto", "product", "delete"),

                //STOCK & STOCKMOVEMENT
                new("read.stock", "Ver stock", "stock", "read"),
                new("update.stock", "Atualizar stock", "stock", "update"),
                new("create.stockmovement", "Criar movimento de stock", "stockmovement", "create"),
                new("read.stockmovement", "Ver movimentos de stock", "stockmovement", "read"),

                //ORDER
                new("read.order", "Ver pedidos", "order", "read"),
                new("update.order", "Atualizar pedido", "order", "update"),
                new("delete.order", "Apagar pedido", "order", "delete"),
                new("cancel.order", "Cancelar pedido", "order", "cancel"),

                //CART
                new("read.cart", "Ver carrinho", "cart", "read"),
                new("update.cart", "Atualizar carrinho", "cart", "update"),
                new("delete.cart", "Apagar carrinho", "cart", "delete"),

                //PAYMENT
                new("create.payment", "Criar pagamento", "payment", "create"),
                new("read.payment", "Ver pagamento", "payment", "read"),
                new("update.payment", "Atualizar pagamento", "payment", "update"),
                new("refund.payment", "Reembolsar pagamento", "payment", "refund"),

                //TENANT
                new("read.tenant", "Ver tenant", "tenant", "read"),
                new("update.tenant", "Atualizar tenant", "tenant", "update"),
                new("delete.tenant", "Apagar tenant", "tenant", "delete"),

                //CUSTOMER
                new("read.customer", "Ler clientes", "customer", "read"),
                new("create.customer", "Criar cliente", "customer", "create"),
                new("update.customer", "Atualizar cliente", "customer", "update"),
                new("delete.customer", "Eliminar cliente", "customer", "delete"),

                //EMPLOYEE
                new("read.employee", "Ler colaboradores", "employee", "read"),
                new("create.employee", "Criar colaborador", "employee", "create"),
                new("update.employee", "Atualizar outro colaborador", "employee", "update"),
                new("delete.employee", "Eliminar colaborador", "employee", "delete"),

                //ROLE
                new("assign.role", "Atribuir role", "role", "assign"),
                new("remove.role", "Remover role", "role", "remove"),
                new("read.role", "Ver roles", "role", "read"),
                new("create.role", "Criar role", "role", "create"),
                new("update.role", "Atualizar role", "role", "update"),
                new("delete.role", "Apagar role", "role", "delete"),

                //PERMISSION
                new("read.permission", "Ler permissions", "permission", "read")
            };

            context.Permissions.AddRange(permissions);
            await context.SaveChangesAsync();
        }
    }
}