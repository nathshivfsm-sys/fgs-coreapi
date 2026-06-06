using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupServiceAssetModelReferenceConfiguration
    : IEntityTypeConfiguration<FgsSetupServiceAssetModelReference>
{
    public void Configure(EntityTypeBuilder<FgsSetupServiceAssetModelReference> entity)
    {
        entity.ToTable("FgsSetupServiceAssetModelReference");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.Property(e => e.UrlsJson).HasColumnType("jsonb");
        entity.HasOne(e => e.ServiceAssetType)
            .WithMany()
            .HasForeignKey(e => e.FgsSetupServiceAssetTypeId)
            .HasConstraintName("FK_FgsSvcAssetModelRef_AssetType")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.ServiceAssetManufacturer)
            .WithMany()
            .HasForeignKey(e => e.FgsSetupServiceAssetManufacturerId)
            .HasConstraintName("FK_FgsSvcAssetModelRef_Mfr")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => e.FgsSetupServiceAssetTypeId)
            .HasDatabaseName("IX_FgsSvcAssetModelRef_TypeId");
        entity.HasIndex(e => e.FgsSetupServiceAssetManufacturerId)
            .HasDatabaseName("IX_FgsSvcAssetModelRef_MfrId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupServiceAssetManufacturerId })
            .HasDatabaseName("IX_FgsSvcAssetModelRef_Mfr");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupServiceAssetTypeId })
            .HasDatabaseName("IX_FgsSvcAssetModelRef_Type");
        entity.HasIndex(e => new
            {
                e.TenantId,
                e.CompanyId,
                e.FgsSetupServiceAssetTypeId,
                e.FgsSetupServiceAssetManufacturerId
            })
            .HasDatabaseName("IX_FgsSvcAssetModelRef_TypeMfr");
        entity.HasIndex(e => e.UrlsJson)
            .HasDatabaseName("IX_ServiceAsset_UrlsJson")
            .HasMethod("gin");
        entity.ToTable(t => t.HasCheckConstraint(
            "CK_FgsSvcAssetModelRef_UrlsJson",
            "\"UrlsJson\" IS NULL OR jsonb_typeof(\"UrlsJson\") = 'array'"));
    }
}
