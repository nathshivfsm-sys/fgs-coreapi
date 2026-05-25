using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsInventoryItemConfiguration : IEntityTypeConfiguration<FgsInventoryItem>
{
    public void Configure(EntityTypeBuilder<FgsInventoryItem> entity)
    {
        entity.ToTable(
            "FgsInventoryItem",
            t => t.HasComment("Inventory item master record for purchasing, sales, and stock tracking."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.ConfigureTenantCompanySetupFk("FK_FgsInventoryItem_FgsTenantCompany_TenantId_CompanyId");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.ItemCode })
            .HasName("UQ_FgsInventoryItem_TenantId_CompanyId_ItemCode");

        entity.Property(e => e.ItemCode).HasMaxLength(100).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.PurchaseDescription).HasColumnType("text");
        entity.Property(e => e.SalesDescription).HasColumnType("text");
        entity.Property(e => e.ManufacturerPartNumber).HasMaxLength(100);
        entity.Property(e => e.UPCCode).HasMaxLength(100);
        entity.Property(e => e.UnitOfMeasure).HasMaxLength(50);
        entity.Property(e => e.TrackQuantity).HasDefaultValue(false);
        entity.Property(e => e.Cost)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0m);
        entity.Property(e => e.SalesPrice)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0m);
        entity.Property(e => e.DefaultTaxable).HasDefaultValue(true);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsInventoryItem_TenantId_CompanyId_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryItemTypeId })
            .HasDatabaseName("IX_FgsInventoryItem_TenantId_CompanyId_InventoryItemTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryCategoryId })
            .HasDatabaseName("IX_FgsInventoryItem_TenantId_CompanyId_InventoryCategoryId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryCategoryId, e.InventorySubCategoryId })
            .HasDatabaseName("IX_FgsInventoryItem_TenantId_CompanyId_InventoryCategoryId_InventorySubCategoryId");

        entity.HasOne<FgsInventoryItemType>()
            .WithMany()
            .HasForeignKey(e => e.InventoryItemTypeId)
            .HasConstraintName("FK_FgsInventoryItem_FgsInventoryItemType")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInventoryCategory>()
            .WithMany()
            .HasForeignKey(e => e.InventoryCategoryId)
            .HasConstraintName("FK_FgsInventoryItem_FgsInventoryCategory")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInventorySubCategory>()
            .WithMany()
            .HasForeignKey(e => e.InventorySubCategoryId)
            .HasConstraintName("FK_FgsInventoryItem_FgsInventorySubCategory")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
