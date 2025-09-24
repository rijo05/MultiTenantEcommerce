using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Infrastructure.Events;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class ProcessedEventsConfiguration : IEntityTypeConfiguration<ProcessedEvent>
{
    public void Configure(EntityTypeBuilder<ProcessedEvent> builder)
    {
        builder.HasKey(e => new { e.EventId , e.HandlerName});
    }
}
