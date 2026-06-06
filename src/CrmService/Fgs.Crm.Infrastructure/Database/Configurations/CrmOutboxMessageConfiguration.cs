using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Domain.Enums;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmOutboxMessageConfiguration : IEntityTypeConfiguration<CrmOutboxMessage>
{
    public void Configure(EntityTypeBuilder<CrmOutboxMessage> entity)
    {
        entity.ToTable("CrmOutboxMessage");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("bigint")
            .UseIdentityByDefaultColumn();

        entity.Ignore(e => e.IsActive);

        entity.Property(e => e.TenantId).HasColumnType("bigint");
        entity.Property(e => e.CompanyId).HasColumnType("bigint");
        entity.Property(e => e.EventType).HasMaxLength(150);
        entity.Property(e => e.AggregateType).HasMaxLength(100);
        entity.Property(e => e.AggregateId).HasMaxLength(100);
        entity.Property(e => e.CorrelationId).HasColumnType("uuid");
        entity.Property(e => e.CausationId).HasColumnType("uuid");
        entity.Property(e => e.ExchangeName).HasMaxLength(200);
        entity.Property(e => e.RoutingKey).HasMaxLength(200);
        entity.Property(e => e.Payload).HasColumnType("jsonb");
        entity.Property(e => e.Headers).HasColumnType("jsonb");
        entity.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(OutboxMessageStatus.Pending);
        entity.Property(e => e.RetryCount).HasDefaultValue(0);
        entity.Property(e => e.MaxRetryCount).HasDefaultValue(10);
        entity.Property(e => e.NextRetryOn).HasColumnType("timestamptz");
        entity.Property(e => e.ProcessedOn).HasColumnType("timestamptz");
        entity.Property(e => e.LastError).HasColumnType("text");
        entity.ConfigureGloEntityAuditColumns();

        entity.HasIndex(e => new { e.Status, e.NextRetryOn })
            .HasDatabaseName("IX_CrmOutboxMessage_Status_NextRetryOn");
        entity.HasIndex(e => e.EventType)
            .HasDatabaseName("IX_CrmOutboxMessage_EventType");
        entity.HasIndex(e => e.TenantId)
            .HasDatabaseName("IX_CrmOutboxMessage_TenantId");
        entity.HasIndex(e => e.CorrelationId)
            .HasDatabaseName("IX_CrmOutboxMessage_CorrelationId");
    }
}
