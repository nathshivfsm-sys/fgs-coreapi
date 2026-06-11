using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupServiceAgreementPricingComponentConfiguration
    : IEntityTypeConfiguration<FgsSetupServiceAgreementPricingComponent>
{
    public void Configure(EntityTypeBuilder<FgsSetupServiceAgreementPricingComponent> entity)
    {
        entity.ToTable(
            "FgsSetupServiceAgreementPricingComponent",
            t =>
            {
                t.HasComment(
                    "Stores reusable service agreement pricing components and their default pricing for a tenant-company. " +
                    "Component types: FIXED (applied once), FIRST_SYSTEM through FIFTH_SYSTEM (per-system tier), ADDITIONAL_SYSTEM (sixth and subsequent systems).");
                t.HasCheckConstraint(
                    "CK_FgsSetupServiceAgreementPricingComponent_TypeCode",
                    "\"PricingComponentTypeCode\" IN ('FIXED','FIRST_SYSTEM','SECOND_SYSTEM','THIRD_SYSTEM','FOURTH_SYSTEM','FIFTH_SYSTEM','ADDITIONAL_SYSTEM')");
                t.HasCheckConstraint(
                    "CK_FgsSetupServiceAgreementPricingComponent_Amount",
                    "\"Amount\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.PricingComponentCode).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.PricingComponentTypeCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment(
                "FIXED, FIRST_SYSTEM, SECOND_SYSTEM, THIRD_SYSTEM, FOURTH_SYSTEM, FIFTH_SYSTEM, or ADDITIONAL_SYSTEM.");
        entity.Property(e => e.Amount).HasPrecision(18, 2).IsRequired();
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.PricingComponentCode })
            .HasName("UQ_FgsSetupServiceAgreementPricingComponent_TenantId_CompanyId_PricingComponentCode");
    }
}
