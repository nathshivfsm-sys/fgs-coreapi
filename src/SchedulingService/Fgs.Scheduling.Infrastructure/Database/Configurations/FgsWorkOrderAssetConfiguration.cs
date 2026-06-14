using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsWorkOrderAssetConfiguration : IEntityTypeConfiguration<FgsWorkOrderAsset>
{
    public void Configure(EntityTypeBuilder<FgsWorkOrderAsset> entity)
    {
        entity.ToTable(
            "FgsWorkOrderAsset",
            t => t.HasComment("Associates assets with a work order."));

        entity.HasKey(e => new { e.TenantId, e.CompanyId, e.WorkOrderId, e.AssetId })
            .HasName("PK_FgsWorkOrderAsset");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.WorkOrderId).HasComment("Parent work order identifier.");
        entity.Property(e => e.AssetId).HasComment("Asset identifier. References asset service; no FK by design.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100).HasComment("User who created the record.");

        entity.HasOne<FgsWorkOrder>()
            .WithMany()
            .HasForeignKey(e => e.WorkOrderId)
            .HasConstraintName("FK_FgsWorkOrderAsset_WorkOrder")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
