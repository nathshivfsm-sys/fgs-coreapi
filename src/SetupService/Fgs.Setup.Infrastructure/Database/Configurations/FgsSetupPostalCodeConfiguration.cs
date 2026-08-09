using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPostalCodeConfiguration : IEntityTypeConfiguration<FgsSetupPostalCode>
{
    public void Configure(EntityTypeBuilder<FgsSetupPostalCode> entity)
    {
        entity.ToTable("FgsSetupPostalCode");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.Property(e => e.PostalCode).IsRequired();
        entity.Property(e => e.CountryCode)
            .HasMaxLength(2)
            .IsRequired()
            .HasComment(
                "ISO 3166-1 alpha-2 country code associated with the postal code (for example, US, CA, MX).");
        entity.Property(e => e.StateProvinceCode)
            .HasMaxLength(10)
            .IsRequired()
            .HasComment(
                "State, province, or territory code associated with the postal code (for example, TX, ON, BC).");
        entity.Property(e => e.City)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("Primary city or municipality associated with the postal code.");
        entity.Property(e => e.TripChargeAmount)
            .HasPrecision(12, 2)
            .HasDefaultValue(0m)
            .IsRequired()
            .HasComment(
                "Default trip charge applied when providing service to this postal code. Used by dispatching, estimating, and pricing calculations.");
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
