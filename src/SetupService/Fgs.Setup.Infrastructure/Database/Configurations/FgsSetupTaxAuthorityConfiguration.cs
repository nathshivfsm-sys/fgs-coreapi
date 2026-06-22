using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupTaxAuthorityConfiguration : IEntityTypeConfiguration<FgsSetupTaxAuthority>
{
    public void Configure(EntityTypeBuilder<FgsSetupTaxAuthority> entity)
    {
        entity.ToTable("FgsSetupTaxAuthority");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupTaxAuthority");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RegionCode })
            .HasDatabaseName("IX_FgsSetupTaxAuthority_RegionCode");
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsSetupTaxAuthority_Code_Upper",
                "\"Code\" = UPPER(\"Code\")");
            t.HasCheckConstraint(
                "CK_FgsSetupTaxAuthority_RegionCode_Upper",
                "\"RegionCode\" IS NULL OR \"RegionCode\" = UPPER(\"RegionCode\")");
            t.HasCheckConstraint(
                "CK_FgsSetupTaxAuthority_TaxPercent",
                "\"TaxPercent\" >= 0 AND \"TaxPercent\" <= 100");
        });
    }
}
