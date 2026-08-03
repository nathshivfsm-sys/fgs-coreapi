using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsPurchaseOrderDetailConfiguration : IEntityTypeConfiguration<FgsPurchaseOrderDetail>
{
    public void Configure(EntityTypeBuilder<FgsPurchaseOrderDetail> entity)
    {
        entity.ToTable(
            "FgsPurchaseOrderDetail",
            t => t.HasComment("Stores purchase order line items for inventory purchased from vendors."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.LineNumber).IsRequired();
        entity.Property(e => e.VendorPartNumber).HasMaxLength(100);
        entity.Property(e => e.ItemDescription).HasMaxLength(255).IsRequired()
            .HasComment("Description printed on the purchase order.");
        entity.Property(e => e.UnitOfMeasureCode).HasMaxLength(25).IsRequired();
        entity.Property(e => e.OrderedQuantity).HasColumnType("numeric(18,4)").HasDefaultValue(0m);
        entity.Property(e => e.ReceivedQuantity).HasColumnType("numeric(18,4)").HasDefaultValue(0m);
        entity.Property(e => e.UnitCost).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Unit cost at the time the purchase order was created.");
        entity.Property(e => e.DiscountAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.IsTaxable).HasDefaultValue(true);
        entity.Property(e => e.ExtendedAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Extended amount calculated from quantity, unit cost and discount before document-level taxes and freight.");
        entity.Property(e => e.ExpectedDeliveryDate).HasColumnType("timestamptz");
        entity.Property(e => e.Notes).HasColumnType("text");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.PurchaseOrderId, e.LineNumber })
            .HasName("UQ_FgsPurchaseOrderDetail_TenantId_CompanyId_PurchaseOrderId_LineNumber");

        entity.HasOne<FgsInventoryItem>()
            .WithMany()
            .HasForeignKey(e => e.ItemId)
            .HasConstraintName("FK_FgsPurchaseOrderDetail_FgsInventoryItem")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsPurchaseOrderDetail_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PurchaseOrderId })
            .HasDatabaseName("IX_FgsPurchaseOrderDetail_TenantId_CompanyId_PurchaseOrderId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ItemId })
            .HasDatabaseName("IX_FgsPurchaseOrderDetail_TenantId_CompanyId_ItemId");
    }
}
