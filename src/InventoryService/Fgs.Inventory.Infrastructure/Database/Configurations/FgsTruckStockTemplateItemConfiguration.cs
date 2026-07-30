using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsTruckStockTemplateItemConfiguration : IEntityTypeConfiguration<FgsTruckStockTemplateItem>
{
    public void Configure(EntityTypeBuilder<FgsTruckStockTemplateItem> entity)
    {
        entity.ToTable(
            "FgsTruckStockTemplateItem",
            t =>
            {
                t.HasComment("Defines the inventory items and desired stocking quantities for a truck stock template.");
                t.HasCheckConstraint(
                    "CK_FgsTruckStockTemplateItem_TargetQuantity",
                    "\"TargetQuantity\" >= 0");
                t.HasCheckConstraint(
                    "CK_FgsTruckStockTemplateItem_MinimumQuantity",
                    "\"MinimumQuantity\" >= 0");
                t.HasCheckConstraint(
                    "CK_FgsTruckStockTemplateItem_TargetGreaterThanMinimum",
                    "\"TargetQuantity\" >= \"MinimumQuantity\"");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the truck stock template item.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId)
            .HasComment("Identifies the tenant that owns this truck stock template item.");
        entity.Property(e => e.CompanyId)
            .HasComment("Identifies the company that owns this truck stock template item.");
        entity.Property(e => e.TruckStockTemplateId).IsRequired()
            .HasComment("References the truck stock template that includes this inventory item.");
        entity.Property(e => e.InventoryItemId).IsRequired()
            .HasComment("References the inventory item included in the truck stock template.");
        entity.Property(e => e.TargetQuantity)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0.00m)
            .IsRequired()
            .HasComment("Desired quantity of the inventory item to stock on trucks using this template.");
        entity.Property(e => e.MinimumQuantity)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0.00m)
            .IsRequired()
            .HasComment("Minimum warehouse quantity required before inventory can be transferred during truck commissioning or synchronization.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue(1)
            .IsRequired()
            .HasComment("Controls the display order of inventory items within the truck stock template.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the truck stock template item was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100)
            .HasComment("User who created the truck stock template item.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time when the truck stock template item was last modified.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100)
            .HasComment("User who last modified the truck stock template item.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TruckStockTemplateId, e.InventoryItemId })
            .HasName("UQ_FgsTruckStockTemplateItem_Template_Item");

        entity.HasOne(e => e.InventoryItem)
            .WithMany()
            .HasForeignKey(e => e.InventoryItemId)
            .HasConstraintName("FK_FgsTruckStockTemplateItem_FgsInventoryItem")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TruckStockTemplateId })
            .HasDatabaseName("IX_FgsTruckStockTemplateItem_TenantId_CompanyId_TruckStockTemplateId");
    }
}
