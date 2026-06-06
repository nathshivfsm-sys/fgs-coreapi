using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPostalCodeConfiguration : IEntityTypeConfiguration<FgsSetupPostalCode>
{
    public void Configure(EntityTypeBuilder<FgsSetupPostalCode> entity)
    {
        entity.ToTable("FgsSetupPostalCode");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.PostalCode })
            .HasName("UQ_FgsSetupPostalCode");
        entity.HasOne<FgsSetupZone>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupZoneId)
            .HasConstraintName("FK_FgsSetupPostalCode_Zone")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<FgsSetupTax>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupTaxId)
            .HasConstraintName("FK_FgsSetupPostalCode_Tax")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => e.FgsSetupZoneId)
            .HasDatabaseName("IX_FgsSetupPostalCode_ZoneId");
        entity.HasIndex(e => e.FgsSetupTaxId)
            .HasDatabaseName("IX_FgsSetupPostalCode_TaxId");
    }
}
