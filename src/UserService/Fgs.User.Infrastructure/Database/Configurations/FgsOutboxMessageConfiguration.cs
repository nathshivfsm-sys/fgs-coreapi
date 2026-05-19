using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsOutboxMessageConfiguration : IEntityTypeConfiguration<FgsOutboxMessage>
{
    public void Configure(EntityTypeBuilder<FgsOutboxMessage> entity)
    {
        entity.ToTable("FgsOutboxMessage");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.IdempotencyKey).IsUnique();
        entity.HasIndex(e => new { e.Status, e.CreatedOn });
        entity.Property(e => e.EventType).HasMaxLength(200);
        entity.Property(e => e.Payload).HasColumnType("jsonb");
        entity.Property(e => e.IdempotencyKey).HasMaxLength(200);
        entity.Property(e => e.CorrelationId).HasMaxLength(100);
        entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);
        entity.Property(e => e.LastError).HasMaxLength(2000);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.ProcessedOn).HasColumnType("timestamptz");
    }
}
