using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsWorkOrderItemConfiguration : IEntityTypeConfiguration<FgsWorkOrderItem>
{
    public void Configure(EntityTypeBuilder<FgsWorkOrderItem> entity)
    {
        entity.ToTable(
            "FgsWorkOrderItem",
            t =>
            {
                t.HasComment(
                    "Stores materials used on a work order. Items may come from the inventory catalog or be entered manually. Customer billing is stored separately on invoice lines.");
                t.HasCheckConstraint(
                    "CK_FgsWorkOrderItem_Item",
                    "\"InventoryItemId\" IS NOT NULL OR COALESCE(TRIM(BOTH FROM \"ItemName\"), '') <> ''");
                t.HasCheckConstraint("CK_FgsWorkOrderItem_Quantity", "\"Quantity\" > 0");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsWorkOrderItem");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.WorkOrderId).HasComment("Parent work order identifier.");
        entity.Property(e => e.InventoryItemId).HasComment("Inventory item identifier. May be NULL when the item is manually entered.");
        entity.Property(e => e.ItemName).HasMaxLength(200).HasComment("Item name used when the item does not exist in the inventory catalog.");
        entity.Property(e => e.Description).HasColumnType("text").HasComment("Additional item description or technician notes.");
        entity.Property(e => e.Quantity).HasColumnType("numeric(18,2)").HasDefaultValue(1.0m).IsRequired()
            .HasComment("Quantity of material used on the work order.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue(1).IsRequired()
            .HasComment("Display order within the work order item list.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100).HasComment("User who last updated the record.");

        entity.HasOne<FgsWorkOrder>()
            .WithMany()
            .HasForeignKey(e => e.WorkOrderId)
            .HasConstraintName("FK_FgsWorkOrderItem_WorkOrder")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsWorkOrderItem_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WorkOrderId }).HasDatabaseName("IX_FgsWorkOrderItem_WorkOrderId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WorkOrderId, e.DisplayOrder }).HasDatabaseName("IX_FgsWorkOrderItem_DisplayOrder");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryItemId }).HasDatabaseName("IX_FgsWorkOrderItem_InventoryItemId");
    }
}
