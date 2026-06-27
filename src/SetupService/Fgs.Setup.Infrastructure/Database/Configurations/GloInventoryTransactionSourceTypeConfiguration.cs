using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class GloInventoryTransactionSourceTypeConfiguration : IEntityTypeConfiguration<GloInventoryTransactionSourceType>
{
    public void Configure(EntityTypeBuilder<GloInventoryTransactionSourceType> entity)
    {
        entity.ToTable(
            "GloInventoryTransactionSourceType",
            t => t.HasComment("Defines business processes and source documents that generate inventory transactions."));

        entity.HasKey(e => e.Id).HasName("PK_GloInventoryTransactionSourceType");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();

        entity.Property(e => e.Code).HasMaxLength(50).IsRequired()
            .HasComment("System code used internally by the application.");
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired()
            .HasComment("Display name of the inventory transaction source type.");
        entity.Property(e => e.Description).HasMaxLength(500)
            .HasComment("Description of the business process that generates inventory transactions.");
        entity.Property(e => e.SortOrder).HasDefaultValue(1)
            .HasComment("Display order.");
        entity.Property(e => e.IsSystem).HasDefaultValue(true);
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the source type is active.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");

        entity.HasIndex(e => e.Code)
            .IsUnique()
            .HasDatabaseName("UQ_GloInventoryTransactionSourceType_Code");
    }
}
