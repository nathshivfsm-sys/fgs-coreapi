using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixLaborConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrixLabor>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrixLabor> entity)
    {
        entity.ToTable(
            "FgsSetupPricingMatrixLabor",
            t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixLabor_BaseRate",
                    "\"BaseRate\" >= 0");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixLabor_DiscountPercent",
                    "\"DiscountPercent\" IS NULL OR (\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100)");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixLabor_OvertimeMultiplier",
                    "\"OvertimeMultiplier\" IS NULL OR \"OvertimeMultiplier\" >= 1");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixLabor_DoubleTimeMultiplier",
                    "\"DoubleTimeMultiplier\" IS NULL OR \"DoubleTimeMultiplier\" >= 1");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.BaseRate).HasPrecision(18, 2);
        entity.Property(e => e.OvertimeMultiplier).HasPrecision(18, 2);
        entity.Property(e => e.DoubleTimeMultiplier).HasPrecision(18, 2);
        entity.Property(e => e.DiscountPercent).HasPrecision(18, 2);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasAlternateKey(e => new
            {
                e.TenantId,
                e.CompanyId,
                e.PricingMatrixId,
                e.LaborRateTypeId,
                e.TechSkillLevelId,
            })
            .HasName("UQ_FgsSetupPricingMatrixLabor");

        entity.HasOne<FgsSetupPricingMatrix>()
            .WithMany()
            .HasForeignKey(e => e.PricingMatrixId)
            .HasConstraintName("FK_FgsSetupPricingMatrixLabor_PricingMatrix")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<FgsSetupTechSkillLevel>()
            .WithMany()
            .HasForeignKey(e => e.TechSkillLevelId)
            .HasConstraintName("FK_FgsSetupPricingMatrixLabor_TechSkillLevel")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PricingMatrixId })
            .HasDatabaseName("IX_FgsSetupPricingMatrixLabor_TenantId_CompanyId_PricingMatrixId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LaborRateTypeId })
            .HasDatabaseName("IX_FgsSetupPricingMatrixLabor_TenantId_CompanyId_LaborRateTypeId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TechSkillLevelId })
            .HasDatabaseName("IX_FgsSetupPricingMatrixLabor_TenantId_CompanyId_TechSkillLevelId");
    }
}
