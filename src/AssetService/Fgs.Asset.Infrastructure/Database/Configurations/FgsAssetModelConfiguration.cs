using Fgs.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal sealed class FgsAssetModelConfiguration : IEntityTypeConfiguration<FgsAssetModel>
{
    public void Configure(EntityTypeBuilder<FgsAssetModel> entity)
    {
        entity.ToTable(
            "FgsAssetModel",
            t => t.HasComment("Catalog of equipment models that may be associated with service assets."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Unique asset model identifier.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.AssetTypeId).HasComment("Asset type associated with this model.");
        entity.Property(e => e.AssetManufacturerId).HasComment("Manufacturer associated with this model.");
        entity.Property(e => e.ModelNumber).HasMaxLength(100).IsRequired()
            .HasComment("Manufacturer model number.");
        entity.Property(e => e.ModelDescription).HasMaxLength(500).IsRequired()
            .HasComment("Description of the model.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the model is active and available for selection.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<FgsAssetType>()
            .WithMany()
            .HasForeignKey(e => e.AssetTypeId)
            .HasConstraintName("FK_FgsAssetModel_AssetType")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsAssetManufacturer>()
            .WithMany()
            .HasForeignKey(e => e.AssetManufacturerId)
            .HasConstraintName("FK_FgsAssetModel_AssetManufacturer")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsAssetModel_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetTypeId })
            .HasDatabaseName("IX_FgsAssetModel_TenantId_CompanyId_AssetTypeId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetManufacturerId })
            .HasDatabaseName("IX_FgsAssetModel_TenantId_CompanyId_AssetManufacturerId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetTypeId, e.AssetManufacturerId })
            .HasDatabaseName("IX_FgsAssetModel_TenantId_CompanyId_AssetTypeId_AssetManufacturerId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsAssetModel_TenantId_CompanyId_IsActive");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetManufacturerId, e.ModelNumber })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAssetModel_TenantCompanyManufacturerModelNumber");
    }
}
