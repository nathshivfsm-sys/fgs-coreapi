using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrix>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrix> entity)
    {
        entity.ToTable(
            "FgsSetupPricingMatrix",
            t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrix_EffectiveDates",
                    "\"EffectiveTo\" IS NULL OR \"EffectiveTo\" >= \"EffectiveFrom\"");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrix_PriceAdjustmentTypeId",
                    "\"PriceAdjustmentTypeId\" BETWEEN 1 AND 3");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.IsLaborTierStructure)
            .HasDefaultValue(false)
            .HasComment(
                "Indicates whether labor pricing in this pricing matrix is based on labor tiers. When false, standard labor pricing rules are applied. When true, labor charges are calculated using the configured labor tier structure.");
        entity.Property(e => e.IsLaborRateBySkillLevel).HasDefaultValue(false);
        entity.Property(e => e.EffectiveFrom).HasColumnType("date");
        entity.Property(e => e.EffectiveTo).HasColumnType("date");
        entity.Property(e => e.IsMobileVisible).HasDefaultValue(true);
        entity.Property(e => e.IsDefault).HasDefaultValue(true);
        entity.Property(e => e.PriceAdjustmentTypeId)
            .HasColumnType("smallint")
            .HasComment("Pricing adjustment method. Valid values: 1=Markup Percent, 2=Markup Amount, 3=Multiplier.");
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupPricingMatrix_TenantId_CompanyId_Code");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsSetupPricingMatrix_TenantId_CompanyId_Name");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSetupPricingMatrix_TenantId_CompanyId_IsActive");
    }
}
