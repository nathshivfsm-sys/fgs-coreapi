using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsVendorInventoryItemConfiguration : IEntityTypeConfiguration<FgsVendorInventoryItem>
{
    public void Configure(EntityTypeBuilder<FgsVendorInventoryItem> entity)
    {
        entity.ToTable(
            "FgsVendorInventoryItem",
            t => t.HasComment(
                "Stores vendor-specific inventory item relationships, vendor part information, pricing, and purchasing defaults."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.ConfigureTenantCompanySetupFk("FK_FgsVendorInventoryItem_FgsTenantCompany_TenantId_CompanyId");

        entity.Property(e => e.VendorPartNumber)
            .HasMaxLength(100)
            .HasComment("Vendor-specific part number for the inventory item.");
        entity.Property(e => e.VendorPartName).HasMaxLength(200);
        entity.Property(e => e.LastCost)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0m)
            .HasComment("Last received cost from the vendor based on purchase order receiving.");
        entity.Property(e => e.LastReceivedDate)
            .HasColumnType("timestamptz")
            .HasComment("Last date inventory was received from the vendor.");
        entity.Property(e => e.PurchaseOrderComments)
            .HasColumnType("text")
            .HasComment("Comments automatically copied to purchase orders for this vendor item combination.");
        entity.Property(e => e.IsPreferredVendor)
            .HasDefaultValue(false)
            .HasComment("Indicates whether this vendor is the preferred vendor for the inventory item.");
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

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
