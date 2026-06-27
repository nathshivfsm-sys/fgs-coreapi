using Fgs.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal sealed class FgsAssetWarrantyConfiguration : IEntityTypeConfiguration<FgsAssetWarranty>
{
    public void Configure(EntityTypeBuilder<FgsAssetWarranty> entity)
    {
        entity.ToTable(
            "FgsAssetWarranty",
            t =>
            {
                t.HasComment(
                    "Stores warranty coverage associated with customer assets and equipment.");
                t.HasCheckConstraint(
                    "CK_FgsAssetWarranty_WarrantyType_Upper",
                    "\"WarrantyType\" = upper(\"WarrantyType\")");
                t.HasCheckConstraint(
                    "CK_FgsAssetWarranty_DateRange",
                    "\"EndDate\" >= \"StartDate\"");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsAssetWarranty");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Unique warranty record identifier.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.AssetId).HasComment("Asset covered by the warranty.");
        entity.Property(e => e.WarrantyType).HasMaxLength(75).IsRequired()
            .HasComment(
                "Warranty type such as MANUFACTURER, MANUFACTURER_EXTENDED, LABOR, LABOR_EXTENDED, PARTS, COMPRESSOR, HEAT_EXCHANGER, INSTALLATION, or OTHER.");
        entity.Property(e => e.WarrantyProvider).HasMaxLength(200)
            .HasComment(
                "Manufacturer, contractor, dealer, or third-party organization providing the warranty coverage.");
        entity.Property(e => e.WarrantyNumber).HasMaxLength(100)
            .HasComment("Warranty contract number, policy number, or manufacturer warranty identifier.");
        entity.Property(e => e.RegistrationNumber).HasMaxLength(100)
            .HasComment("Warranty registration confirmation number provided by the warranty issuer.");
        entity.Property(e => e.StartDate).HasColumnType("date").IsRequired()
            .HasComment("Date warranty coverage becomes effective.");
        entity.Property(e => e.EndDate).HasColumnType("date").IsRequired()
            .HasComment("Date warranty coverage expires.");
        entity.Property(e => e.CoverageDescription).HasMaxLength(1000)
            .HasComment(
                "Detailed description of warranty coverage including covered components, labor coverage, exclusions, reimbursement limitations, registration requirements, and special warranty terms.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<Domain.Entities.FgsAsset>()
            .WithMany()
            .HasForeignKey(e => e.AssetId)
            .HasConstraintName("FK_FgsAssetWarranty_Asset")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.AssetId, e.WarrantyType, e.StartDate })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAssetWarranty_AssetId_WarrantyType_StartDate");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsAssetWarranty_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetId })
            .HasDatabaseName("IX_FgsAssetWarranty_TenantId_CompanyId_AssetId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WarrantyType })
            .HasDatabaseName("IX_FgsAssetWarranty_TenantId_CompanyId_WarrantyType");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EndDate })
            .HasDatabaseName("IX_FgsAssetWarranty_TenantId_CompanyId_EndDate");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.StartDate, e.EndDate })
            .HasDatabaseName("IX_FgsAssetWarranty_TenantId_CompanyId_StartDate_EndDate");
    }
}
