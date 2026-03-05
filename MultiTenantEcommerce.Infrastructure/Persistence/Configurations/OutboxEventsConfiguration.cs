using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Infrastructure.Shared.Outbox;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;

public class OutboxEventsConfiguration : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        builder.HasKey(e => e.Id);
    }
}