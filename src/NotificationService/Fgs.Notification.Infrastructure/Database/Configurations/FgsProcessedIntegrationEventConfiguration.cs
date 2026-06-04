using Fgs.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Notification.Infrastructure.Database.Configurations;

internal class FgsProcessedIntegrationEventConfiguration : IEntityTypeConfiguration<FgsProcessedIntegrationEvent>
{
    public void Configure(EntityTypeBuilder<FgsProcessedIntegrationEvent> entity)
    {
        entity.ToTable("FgsProcessedIntegrationEvent");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.MessageId).HasMaxLength(128);
        entity.Property(e => e.EventType).HasMaxLength(128);
        entity.Property(e => e.ProcessedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => e.MessageId)
            .IsUnique()
            .HasDatabaseName("IX_FgsProcessedIntegrationEvent_MessageId");
    }
}
