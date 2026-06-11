using Fgs.ServiceAgreement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.ServiceAgreement.Infrastructure.Database.Configurations;

internal sealed class FgsServiceAgreementVisitItemConfiguration : IEntityTypeConfiguration<FgsServiceAgreementVisitItem>
{
    public void Configure(EntityTypeBuilder<FgsServiceAgreementVisitItem> entity)
    {
        entity.ToTable(
            "FgsServiceAgreementVisitItem",
            t =>
            {
                t.HasComment(
                    "Stores recommended items associated with a service agreement maintenance visit. Items may come from the inventory catalog or be entered manually. Actual material usage is recorded on the work order.");
                t.HasCheckConstraint(
                    "CK_FgsServiceAgreementVisitItem_Item",
                    "\"InventoryItemId\" IS NOT NULL OR COALESCE(TRIM(\"ItemName\"), '') <> ''");
                t.HasCheckConstraint("CK_FgsServiceAgreementVisitItem_Quantity", "\"Quantity\" > 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.ServiceAgreementId).HasComment("Parent service agreement identifier.");
        entity.Property(e => e.ServiceAgreementVisitId).HasComment("Parent service agreement visit identifier.");
        entity.Property(e => e.InventoryItemId).HasComment("Inventory item identifier. May be NULL when the item is manually entered.");
        entity.Property(e => e.ItemName).HasMaxLength(200).HasComment("Item name used when the item does not exist in the inventory catalog.");
        entity.Property(e => e.Description).HasColumnType("text").HasComment("Additional item description or maintenance instructions.");
        entity.Property(e => e.Quantity).HasColumnType("numeric(18,2)").HasDefaultValue(1m)
            .HasComment("Expected quantity required for the maintenance visit.");
        entity.Property(e => e.IsRequired).HasDefaultValue(true).HasComment("Indicates whether the item is required for the maintenance visit.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue(1).HasComment("Display order within the maintenance visit item list.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");
        entity.Property(e => e.CreatedBy).HasComment("User that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Record last update timestamp.");
        entity.Property(e => e.UpdatedBy).HasComment("User that last updated the record.");

        entity.HasOne(e => e.ServiceAgreementVisit)
            .WithMany()
            .HasForeignKey(e => e.ServiceAgreementVisitId)
            .HasConstraintName("FK_FgsServiceAgreementVisitItem_ServiceAgreementVisit")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsServiceAgreementVisitItem_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId })
            .HasDatabaseName("IX_FgsServiceAgreementVisitItem_ServiceAgreementId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementVisitId })
            .HasDatabaseName("IX_FgsServiceAgreementVisitItem_ServiceAgreementVisitId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryItemId })
            .HasDatabaseName("IX_FgsServiceAgreementVisitItem_InventoryItemId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsRequired })
            .HasDatabaseName("IX_FgsServiceAgreementVisitItem_IsRequired");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementVisitId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsServiceAgreementVisitItem_DisplayOrder");
    }
}
