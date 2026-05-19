using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsSetupTaxDetailConfiguration : IEntityTypeConfiguration<FgsSetupTaxDetail>
{
    public void Configure(EntityTypeBuilder<FgsSetupTaxDetail> entity)
    {
        entity.ToTable("FgsSetupTaxDetail");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.ConfigureTenantCompanySetupFk("FK_FgsSetupTaxDetail_FgsTenantCompany_TenantId_CompanyId");
        entity.HasOne<FgsSetupTax>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupTaxId)
            .HasConstraintName("FK_FgsSetupTaxDetail_Tax")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<FgsSetupTaxAuthority>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupTaxAuthorityId)
            .HasConstraintName("FK_FgsSetupTaxDetail_TaxAuth")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupTaxId, e.EffectiveFromDate, e.EffectiveToDate })
            .HasDatabaseName("IX_FgsSetupTaxDetail_Tax");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupTaxAuthorityId })
            .HasDatabaseName("IX_FgsSetupTaxDetail_TaxAuth");
        entity.HasIndex(e => new { e.EffectiveFromDate, e.EffectiveToDate })
            .HasDatabaseName("IX_FgsSetupTaxDetail_EffectiveDates");
        entity.HasIndex(e => e.FgsSetupTaxId)
            .HasDatabaseName("IX_FgsSetupTaxDetail_TaxId");
        entity.HasIndex(e => e.FgsSetupTaxAuthorityId)
            .HasDatabaseName("IX_FgsSetupTaxDetail_TaxAuthId");
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsSetupTaxDetail_TaxPercent",
                "\"TaxPercent\" >= 0 AND \"TaxPercent\" <= 100");
            t.HasCheckConstraint(
                "CK_FgsSetupTaxDetail_EffectiveDates",
                "\"EffectiveToDate\" IS NULL OR \"EffectiveToDate\" >= \"EffectiveFromDate\"");
        });
    }
}
