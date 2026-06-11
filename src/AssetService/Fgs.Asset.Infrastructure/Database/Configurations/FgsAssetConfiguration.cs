using Fgs.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal sealed class FgsAssetConfiguration : IEntityTypeConfiguration<Domain.Entities.FgsAsset>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.FgsAsset> entity)
    {
        entity.ToTable(
            "FgsAsset",
            t => t.HasComment(
                "Stores customer-owned equipment and installed assets at service locations."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Unique asset identifier.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.AssetGuid).HasComment("Globally unique asset identifier used by integrations and external systems.");
        entity.Property(e => e.AssetNumber).HasMaxLength(100).IsRequired()
            .HasComment("User-visible asset number within the tenant company.");
        entity.Property(e => e.ServiceLocationId).HasComment("Service location where the asset is installed.");
        entity.Property(e => e.AssetTypeId).HasComment("Optional catalog asset type reference.");
        entity.Property(e => e.AssetManufacturerId).HasComment("Optional catalog manufacturer reference.");
        entity.Property(e => e.AssetModelId).HasComment("Optional catalog model reference.");
        entity.Property(e => e.AssetDescription).HasColumnType("text")
            .HasComment("Internal asset description.");
        entity.Property(e => e.CustomerAssetNumber).HasMaxLength(100)
            .HasComment("Customer-provided asset number or tag.");
        entity.Property(e => e.CustomerAssetName).HasMaxLength(200)
            .HasComment("Customer-provided asset name.");
        entity.Property(e => e.ManufacturerName).HasMaxLength(200)
            .HasComment("Free-text manufacturer name when not linked to catalog.");
        entity.Property(e => e.ModelNumber).HasMaxLength(100)
            .HasComment("Free-text model number when not linked to catalog.");
        entity.Property(e => e.SerialNumber).HasMaxLength(200)
            .HasComment("Equipment serial number.");
        entity.Property(e => e.ManufactureDate).HasColumnType("date")
            .HasComment("Date the equipment was manufactured.");
        entity.Property(e => e.InstallDate).HasColumnType("date")
            .HasComment("Date the asset was installed at the service location.");
        entity.Property(e => e.InstalledWorkOrderId).HasComment("Work order that installed the asset. References job service; no FK by design.");
        entity.Property(e => e.IsInstalledByCompany).HasDefaultValue(false)
            .HasComment("Indicates whether the asset was installed by the service company.");
        entity.Property(e => e.AssetStatusId).HasComment("Current lifecycle status of the asset.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the asset record is active.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.ConfigureServiceLocationCacheFk("FK_FgsAsset_ServiceLocationCache");

        entity.HasOne<FgsAssetType>()
            .WithMany()
            .HasForeignKey(e => e.AssetTypeId)
            .HasConstraintName("FK_FgsAsset_AssetType")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsAssetManufacturer>()
            .WithMany()
            .HasForeignKey(e => e.AssetManufacturerId)
            .HasConstraintName("FK_FgsAsset_AssetManufacturer")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsAssetModel>()
            .WithMany()
            .HasForeignKey(e => e.AssetModelId)
            .HasConstraintName("FK_FgsAsset_AssetModel")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsAssetStatus>()
            .WithMany()
            .HasForeignKey(e => e.AssetStatusId)
            .HasConstraintName("FK_FgsAsset_AssetStatus")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => e.AssetGuid).IsUnique().HasDatabaseName("UQ_FgsAsset_AssetGuid");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetNumber })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAsset_TenantId_CompanyId_AssetNumber");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceLocationId })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId_ServiceLocationId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetTypeId })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId_AssetTypeId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetManufacturerId })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId_AssetManufacturerId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetModelId })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId_AssetModelId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetStatusId })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId_AssetStatusId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SerialNumber })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId_SerialNumber");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InstalledWorkOrderId })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId_InstalledWorkOrderId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsInstalledByCompany })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId_IsInstalledByCompany");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsAsset_TenantId_CompanyId_IsActive");
    }
}
