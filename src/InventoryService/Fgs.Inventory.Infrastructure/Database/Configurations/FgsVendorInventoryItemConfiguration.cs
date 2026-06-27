using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsVendorInventoryItemConfiguration : IEntityTypeConfiguration<FgsVendorInventoryItem>
{
    public void Configure(EntityTypeBuilder<FgsVendorInventoryItem> entity)
    {
        entity.ToTable(
            "FgsVendorInventoryItem",
            t => t.HasComment(
                "Stores vendor-specific purchasing information for inventory items, including vendor part numbers, descriptions, pricing, purchasing priority, lead times, and other information used during purchase order creation and inventory replenishment."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureCatalogColumns();

        entity.Property(e => e.VendorPartNumber)
            .HasMaxLength(100)
            .HasComment("Vendor's part number used when purchasing this inventory item.");
        entity.Property(e => e.VendorPartName)
            .HasMaxLength(200)
            .HasComment("Vendor's description of the inventory item as it appears on catalogs or purchase orders.");
        entity.Property(e => e.LastCost)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0m)
            .HasComment("Most recent purchase cost from this vendor for the inventory item.");
        entity.Property(e => e.LastReceivedDate)
            .HasColumnType("timestamptz")
            .HasComment("Date the inventory item was last received from this vendor.");
        entity.Property(e => e.PurchaseOrderComments)
            .HasColumnType("text")
            .HasComment("Vendor-specific notes automatically included or displayed during purchase order creation for this inventory item.");
        entity.Property(e => e.VendorPriority)
            .HasComment("Specifies the purchasing priority for this vendor and inventory item combination. Lower numbers indicate higher priority.");
        entity.Property(e => e.LeadTimeDays)
            .HasComment("Expected number of days required for the vendor to deliver the inventory item after the purchase order is placed.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.VendorId, e.InventoryItemId })
            .HasName("UQ_FgsVendorInventoryItem_TenantId_CompanyId_VendorId_InventoryItemId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.VendorId })
            .HasDatabaseName("IX_FgsVendorInventoryItem_TenantId_CompanyId_VendorId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryItemId })
            .HasDatabaseName("IX_FgsVendorInventoryItem_TenantId_CompanyId_InventoryItemId");

        entity.HasOne<FgsVendor>()
            .WithMany()
            .HasForeignKey(e => e.VendorId)
            .HasConstraintName("FK_FgsVendorInventoryItem_FgsVendor")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInventoryItem>()
            .WithMany()
            .HasForeignKey(e => e.InventoryItemId)
            .HasConstraintName("FK_FgsVendorInventoryItem_FgsInventoryItem")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
