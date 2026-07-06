using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsInventoryStockConfiguration : IEntityTypeConfiguration<FgsInventoryStock>
{
    public void Configure(EntityTypeBuilder<FgsInventoryStock> entity)
    {
        entity.ToTable("FgsInventoryStock");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.InventoryItemId })
            .HasName("UQ_FgsInventoryStock_TenantId_CompanyId_InventoryItemId");

        entity.Property(e => e.QuantityOnHand).HasColumnType("numeric(18,4)").HasDefaultValue(0m);
        entity.Property(e => e.QuantityCommitted).HasColumnType("numeric(18,4)").HasDefaultValue(0m);
        entity.Property(e => e.QuantityAvailable).HasColumnType("numeric(18,4)").HasDefaultValue(0m);
        entity.Property(e => e.AverageCost).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.LastCost).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.LastPurchaseDate).HasColumnType("timestamptz");
        entity.Property(e => e.LastSoldDate).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        entity.HasOne<FgsInventoryItem>()
            .WithMany()
            .HasForeignKey(e => e.InventoryItemId)
            .HasConstraintName("FK_FgsInventoryStock_FgsInventoryItem")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInventoryStock_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryItemId })
            .HasDatabaseName("IX_FgsInventoryStock_TenantId_CompanyId_InventoryItemId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LastPurchaseDate })
            .HasDatabaseName("IX_FgsInventoryStock_TenantId_CompanyId_LastPurchaseDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LastSoldDate })
            .HasDatabaseName("IX_FgsInventoryStock_TenantId_CompanyId_LastSoldDate");
    }
}
