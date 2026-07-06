using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsInventoryTransactionConfiguration : IEntityTypeConfiguration<FgsInventoryTransaction>
{
    public void Configure(EntityTypeBuilder<FgsInventoryTransaction> entity)
    {
        entity.ToTable(
            "FgsInventoryTransaction",
            t =>
            {
                t.HasComment(
                    "Stores an immutable audit log of every inventory movement between inventory locations or into and out of inventory.");
                t.HasCheckConstraint(
                    "CK_FgsInventoryTransaction_TransactionType",
                    "\"TransactionType\" IN ('INITIAL', 'PURCHASE_RECEIPT', 'TRANSFER', 'USAGE', 'ADJUSTMENT', 'RETURN', 'PHYSICAL_COUNT')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TransactionNumber).HasMaxLength(50).IsRequired();
        entity.Property(e => e.TransactionType).HasMaxLength(30).IsRequired();
        entity.Property(e => e.Quantity).HasColumnType("numeric(18,4)").IsRequired();
        entity.Property(e => e.UnitCost).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.TransactionDate)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.ReferenceType).HasMaxLength(30);
        entity.Property(e => e.Notes).HasColumnType("text");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TransactionNumber })
            .HasName("UQ_FgsInventoryTransaction_TenantId_CompanyId_TransactionNumber");

        entity.HasOne<FgsInventoryItem>()
            .WithMany()
            .HasForeignKey(e => e.InventoryItemId)
            .HasConstraintName("FK_FgsInventoryTransaction_FgsInventoryItem")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInventoryLocation>()
            .WithMany()
            .HasForeignKey(e => e.FromInventoryLocationId)
            .HasConstraintName("FK_FgsInventoryTransaction_FromInventoryLocation")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInventoryLocation>()
            .WithMany()
            .HasForeignKey(e => e.ToInventoryLocationId)
            .HasConstraintName("FK_FgsInventoryTransaction_ToInventoryLocation")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInventoryTransaction_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryItemId })
            .HasDatabaseName("IX_FgsInventoryTransaction_TenantId_CompanyId_InventoryItemId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TransactionDate })
            .HasDatabaseName("IX_FgsInventoryTransaction_TenantId_CompanyId_TransactionDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TransactionType })
            .HasDatabaseName("IX_FgsInventoryTransaction_TenantId_CompanyId_TransactionType");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ReferenceType, e.ReferenceId })
            .HasDatabaseName("IX_FgsInventoryTransaction_TenantId_CompanyId_ReferenceType_ReferenceId");
    }
}
