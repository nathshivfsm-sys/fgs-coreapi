using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixLaborConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrixLabor>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrixLabor> entity)
    {
        entity.ToTable("FgsSetupPricingMatrixLabor");
        entity.HasKey(e => e.Id);
        entity.ConfigureTenantCompanyGuidSetupColumns();
        entity.ConfigureTenantCompanyGuidSetupFk("FK_FgsSetupPricingMatrixLabor_Company");
        entity.Property(e => e.BaseRate).HasPrecision(18, 2);
        entity.Property(e => e.OvertimeMultiplier).HasPrecision(18, 2);
        entity.Property(e => e.DoubleTimeMultiplier).HasPrecision(18, 2);
        entity.Property(e => e.DiscountPercent).HasPrecision(18, 2);
        entity.HasOne<FgsSetupPricingMatrix>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupPricingMatrixId)
            .HasConstraintName("FK_FgsSetupPricingMatrixLabor_PricingMatrix")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<GloSetupLaborRateType>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupLaborRateTypeId)
            .HasConstraintName("FK_FgsSetupPricingMatrixLabor_LaborRateType")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<FgsSetupTechSkillLevel>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupTechSkillLevelId)
            .HasConstraintName("FK_FgsSetupPricingMatrixLabor_TechSkillLevel")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => e.FgsSetupPricingMatrixId);
        entity.HasIndex(e => e.FgsSetupLaborRateTypeId);
        entity.HasIndex(e => e.FgsSetupTechSkillLevelId);
    }
}
