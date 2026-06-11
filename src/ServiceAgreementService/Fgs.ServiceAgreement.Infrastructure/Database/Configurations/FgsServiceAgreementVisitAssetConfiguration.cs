using Fgs.ServiceAgreement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.ServiceAgreement.Infrastructure.Database.Configurations;

internal sealed class FgsServiceAgreementVisitAssetConfiguration : IEntityTypeConfiguration<FgsServiceAgreementVisitAsset>
{
    public void Configure(EntityTypeBuilder<FgsServiceAgreementVisitAsset> entity)
    {
        entity.ToTable(
            "FgsServiceAgreementVisitAsset",
            t => t.HasComment(
                "Stores assets associated with a service agreement maintenance visit. A visit may include one or more covered assets that require maintenance service."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.ServiceAgreementId).HasComment("Parent service agreement identifier.");
        entity.Property(e => e.ServiceAgreementVisitId).HasComment("Parent service agreement visit identifier.");
        entity.Property(e => e.AssetId).HasComment("Asset associated with the service agreement visit.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");
        entity.Property(e => e.CreatedBy).HasComment("User that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Record last update timestamp.");
        entity.Property(e => e.UpdatedBy).HasComment("User that last updated the record.");

        entity.HasOne(e => e.ServiceAgreementVisit)
            .WithMany()
            .HasForeignKey(e => e.ServiceAgreementVisitId)
            .HasConstraintName("FK_FgsServiceAgreementVisitAsset_ServiceAgreementVisit")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementVisitId, e.AssetId })
            .IsUnique()
            .HasDatabaseName("UQ_FgsServiceAgreementVisitAsset_Visit_Asset");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsServiceAgreementVisitAsset_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId })
            .HasDatabaseName("IX_FgsServiceAgreementVisitAsset_ServiceAgreementId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementVisitId })
            .HasDatabaseName("IX_FgsServiceAgreementVisitAsset_ServiceAgreementVisitId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetId }).HasDatabaseName("IX_FgsServiceAgreementVisitAsset_AssetId");
    }
}
