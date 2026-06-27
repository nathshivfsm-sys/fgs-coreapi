using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupTaxConfiguration : IEntityTypeConfiguration<FgsSetupTax>
{
    public void Configure(EntityTypeBuilder<FgsSetupTax> entity)
    {
        entity.ToTable("FgsSetupTax");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TaxCode })
            .HasName("UQ_FgsSetupTax");
        entity.Property(e => e.ExternalSystemId).HasMaxLength(200);
        entity.Property(e => e.SyncToken).HasMaxLength(100);
        entity.Property(e => e.ShowTaxDetail).HasDefaultValue(false);
        entity.ToTable(t => t.HasCheckConstraint(
            "CK_FgsSetupTax_TaxCode_Upper",
            "\"TaxCode\" = UPPER(\"TaxCode\")"));
    }
}
