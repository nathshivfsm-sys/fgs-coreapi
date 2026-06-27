using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsPurchaseOrderConfiguration : IEntityTypeConfiguration<FgsPurchaseOrder>
{
    public void Configure(EntityTypeBuilder<FgsPurchaseOrder> entity)
    {
        entity.ToTable(
            "FgsPurchaseOrder",
            t =>
            {
                t.HasComment(
                    "Stores purchase order header information including vendor, shipping destination, tax summary and purchasing details.");
                t.HasCheckConstraint(
                    "CK_FgsPurchaseOrder_PurchaseOrderStatus",
                    "\"PurchaseOrderStatus\" IN ('OPEN', 'PARTIAL', 'RECEIVED', 'CLOSED', 'CANCELLED')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(50).IsRequired()
            .HasComment("User-visible purchase order number.");
        entity.Property(e => e.PurchaseOrderStatus).HasMaxLength(30).IsRequired().HasDefaultValue(PurchaseOrderStatuses.Open)
            .HasComment("OPEN, PARTIAL, RECEIVED, CLOSED or CANCELLED.");
        entity.Property(e => e.PurchaseOrderDate)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.ExpectedDeliveryDate).HasColumnType("timestamptz");
        entity.Property(e => e.RequestedByEmployeeId)
            .HasComment("Employee requesting the purchase.");
        entity.Property(e => e.RequestedByName).HasMaxLength(150)
            .HasComment("Snapshot of the requester name when the purchase order was created.");
        entity.Property(e => e.BuyerEmployeeId)
            .HasComment("Employee responsible for purchasing and vendor follow-up.");
        entity.Property(e => e.ShipToInventoryLocationId)
            .HasComment("Inventory location receiving the shipment.");
        entity.Property(e => e.ShipToServiceLocationId)
            .HasComment("Service location or job site receiving the shipment.");
        entity.Property(e => e.ShipToName).HasMaxLength(200);
        entity.Property(e => e.ShipToAddress1).HasMaxLength(200);
        entity.Property(e => e.ShipToAddress2).HasMaxLength(200);
        entity.Property(e => e.ShipToCity).HasMaxLength(100);
        entity.Property(e => e.ShipToStateProvince).HasMaxLength(100);
        entity.Property(e => e.ShipToPostalCode).HasMaxLength(20);
        entity.Property(e => e.ShipToCountry).HasMaxLength(100);
        entity.Property(e => e.VendorReferenceNumber).HasMaxLength(100);
        entity.Property(e => e.VendorContactName).HasMaxLength(150);
        entity.Property(e => e.VendorEmail).HasMaxLength(255);
        entity.Property(e => e.VendorPhoneNumber).HasMaxLength(50);
        entity.Property(e => e.Subtotal).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.DiscountAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.TaxableAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.PurchaseTaxJson).HasColumnType("jsonb")
            .HasComment("JSON tax breakdown supporting multiple tax jurisdictions such as GST, PST, HST, VAT and Sales Tax.");
        entity.Property(e => e.FreightAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.OtherCharges).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.TotalAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.VendorNotes).HasColumnType("text")
            .HasComment("Notes printed on the purchase order for the vendor.");
        entity.Property(e => e.InternalNotes).HasColumnType("text")
            .HasComment("Internal office notes not printed on the purchase order.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.PurchaseOrderNumber })
            .HasName("UQ_FgsPurchaseOrder_TenantId_CompanyId_PurchaseOrderNumber");

        entity.HasOne<FgsVendor>()
            .WithMany()
            .HasForeignKey(e => e.VendorId)
            .HasConstraintName("FK_FgsPurchaseOrder_FgsVendor")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInventoryLocation>()
            .WithMany()
            .HasForeignKey(e => e.ShipToInventoryLocationId)
            .HasConstraintName("FK_FgsPurchaseOrder_FgsInventoryLocation")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsPurchaseOrder_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.VendorId })
            .HasDatabaseName("IX_FgsPurchaseOrder_TenantId_CompanyId_VendorId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PurchaseOrderStatus })
            .HasDatabaseName("IX_FgsPurchaseOrder_TenantId_CompanyId_PurchaseOrderStatus");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PurchaseOrderDate })
            .HasDatabaseName("IX_FgsPurchaseOrder_TenantId_CompanyId_PurchaseOrderDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BuyerEmployeeId })
            .HasDatabaseName("IX_FgsPurchaseOrder_TenantId_CompanyId_BuyerEmployeeId");
    }
}
