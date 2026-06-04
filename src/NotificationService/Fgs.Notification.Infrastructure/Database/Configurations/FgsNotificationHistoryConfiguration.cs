using Fgs.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Notification.Infrastructure.Database.Configurations;

internal class FgsNotificationHistoryConfiguration : IEntityTypeConfiguration<FgsNotificationHistory>
{
    public void Configure(EntityTypeBuilder<FgsNotificationHistory> entity)
    {
        entity.ToTable("FgsNotificationHistory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.TemplateName).HasMaxLength(128);
        entity.Property(e => e.Recipient).HasMaxLength(512);
        entity.Property(e => e.CorrelationId).HasMaxLength(64);
        entity.Property(e => e.ProviderMessageId).HasMaxLength(256);
        entity.Property(e => e.Error).HasMaxLength(2000);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.SentOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CreatedOn })
            .HasDatabaseName("IX_FgsNotificationHistory_TenantId_CreatedOn");
    }
}
