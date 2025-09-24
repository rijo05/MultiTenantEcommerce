using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);

        //builder.HasData(
        //    //CATEGORY
        //    new Permission("read.category", "Ver categoria", "category", "read"),
        //    new Permission("create.category", "Criar categoria", "category", "create"),
        //    new Permission("update.category", "Atualizar categoria", "category", "update"),
        //    new Permission("delete.category", "Apagar categoria", "category", "delete"),

        //    //PRODUCT
        //    new Permission("read.product", "Ver produto", "product", "read"),
        //    new Permission("create.product", "Criar produto", "product", "create"),
        //    new Permission("update.product", "Atualizar produto", "product", "update"),
        //    new Permission("delete.product", "Apagar produto", "product", "delete"),

        //    //STOCK & STOCKMOVEMENT
        //    new Permission("read.stock", "Ver stock", "stock", "read"),
        //    new Permission("update.stock", "Atualizar stock", "stock", "update"),
        //    new Permission("create.stockmovement", "Criar movimento de stock", "stockmovement", "create"),
        //    new Permission("read.stockmovement", "Ver movimentos de stock", "stockmovement", "read"),

        //    //ORDER
        //    new Permission("read.order", "Ver pedidos", "order", "read"),
        //    new Permission("update.order", "Atualizar pedido", "order", "update"),
        //    new Permission("delete.order", "Apagar pedido", "order", "delete"),
        //    new Permission("cancel.order", "Cancelar pedido", "order", "cancel"),

        //    //CART
        //    new Permission("read.cart", "Ver carrinho", "cart", "read"),
        //    new Permission("update.cart", "Atualizar carrinho", "cart", "update"),
        //    new Permission("delete.cart", "Apagar carrinho", "cart", "delete"),

        //    //PAYMENT
        //    new Permission("create.payment", "Criar pagamento", "payment", "create"),
        //    new Permission("read.payment", "Ver pagamento", "payment", "read"),
        //    new Permission("update.payment", "Atualizar pagamento", "payment", "update"),
        //    new Permission("refund.payment", "Reembolsar pagamento", "payment", "refund"),

        //    //TENANT
        //    new Permission("read.tenant", "Ver tenant", "tenant", "read"),
        //    new Permission("update.tenant", "Atualizar tenant", "tenant", "update"),
        //    new Permission("delete.tenant", "Apagar tenant", "tenant", "delete"),

        //    //CUSTOMER
        //    new Permission("read.customer", "Ler clientes", "customer", "read"),
        //    new Permission("create.customer", "Criar cliente", "customer", "create"),
        //    new Permission("update.customer", "Atualizar cliente", "customer", "update"),
        //    new Permission("delete.customer", "Eliminar cliente", "customer", "delete"),

        //    //EMPLOYEE
        //    new Permission("read.employee", "Ler colaboradores", "employee", "read"),
        //    new Permission("create.employee", "Criar colaborador", "employee", "create"),
        //    new Permission("update.employee", "Atualizar outro colaborador", "employee", "update"),
        //    new Permission("delete.employee", "Eliminar colaborador", "employee", "delete"),

        //    //ROLE
        //    new Permission("assign.role", "Atribuir role", "role", "assign"),
        //    new Permission("remove.role", "Remover role", "role", "remove"),
        //    new Permission("read.role", "Ver roles", "role", "read"),
        //    new Permission("create.role", "Criar role", "role", "create"),
        //    new Permission("update.role", "Atualizar role", "role", "update"),
        //    new Permission("delete.role", "Apagar role", "role", "delete")
        //);

    }
}
