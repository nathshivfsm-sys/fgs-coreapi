using Fgs.ServiceAgreement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.ServiceAgreement.Infrastructure.Database.Configurations;

internal sealed class FgsServiceAgreementCoveredAssetConfiguration : IEntityTypeConfiguration<FgsServiceAgreementCoveredAsset>
{
    public void Configure(EntityTypeBuilder<FgsServiceAgreementCoveredAsset> entity)
    {
        entity.ToTable(
            "FgsServiceAgreementCoveredAsset",
            t => t.HasComment(
                "Stores assets covered under a service agreement. Covered assets are entitled to contract benefits such as labor discounts, material discounts, contract pricing, and service agreement coverage. Coverage is inherited from the parent agreement term."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.ServiceAgreementId).HasComment("Parent service agreement identifier.");
        entity.Property(e => e.AssetId).HasComment("Covered customer asset identifier.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");
        entity.Property(e => e.CreatedBy).HasComment("User that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Record last update timestamp.");
        entity.Property(e => e.UpdatedBy).HasComment("User that last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId, e.AssetId })
            .IsUnique()
            .HasDatabaseName("UQ_FgsServiceAgreementCoveredAsset_Agreement_Asset");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsServiceAgreementCoveredAsset_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId })
            .HasDatabaseName("IX_FgsServiceAgreementCoveredAsset_ServiceAgreementId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetId })
            .HasDatabaseName("IX_FgsServiceAgreementCoveredAsset_AssetId");
    }
}
