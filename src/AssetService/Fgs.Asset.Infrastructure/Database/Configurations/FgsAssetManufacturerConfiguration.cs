using Fgs.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal sealed class FgsAssetManufacturerConfiguration : IEntityTypeConfiguration<FgsAssetManufacturer>
{
    public void Configure(EntityTypeBuilder<FgsAssetManufacturer> entity)
    {
        entity.ToTable(
            "FgsAssetManufacturer",
            t =>
            {
                t.HasComment("Stores equipment manufacturers available for asset management.");
                t.HasCheckConstraint("CK_FgsAssetManufacturer_Code_Upper", "\"Code\" = upper(\"Code\")");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Unique asset manufacturer identifier.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.Code).HasMaxLength(75).IsRequired()
            .HasComment("Unique manufacturer code within the tenant company. Stored in uppercase.");
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired()
            .HasComment("Manufacturer name displayed to users.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Optional description of the manufacturer.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the manufacturer is active and available for selection.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsAssetManufacturer_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAssetManufacturer_TenantId_CompanyId_Code");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsAssetManufacturer_TenantId_CompanyId_Name");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsAssetManufacturer_TenantId_CompanyId_IsActive");
    }
}

