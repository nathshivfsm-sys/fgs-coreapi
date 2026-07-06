using Fgs.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal sealed class FgsAssetStatusConfiguration : IEntityTypeConfiguration<FgsAssetStatus>
{
    public void Configure(EntityTypeBuilder<FgsAssetStatus> entity)
    {
        entity.ToTable(
            "assetstatus",
            t =>
            {
                t.HasComment("Stores lifecycle status values used to classify service assets.");
                t.HasCheckConstraint("CK_FgsAssetStatus_Code_Upper", "\"Code\" = upper(\"Code\")");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Unique asset status identifier.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.Code).HasMaxLength(75).IsRequired()
            .HasComment("Unique status code within the tenant company. Stored in uppercase.");
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired()
            .HasComment("Display name of the asset status.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Optional description of the asset status.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the status is active and available for selection.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsAssetStatus_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAssetStatus_TenantId_CompanyId_Code");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsAssetStatus_TenantId_CompanyId_Name");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsAssetStatus_TenantId_CompanyId_IsActive");
    }
}

