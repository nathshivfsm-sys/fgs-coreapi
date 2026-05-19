using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixLaborTierConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrixLaborTier>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrixLaborTier> entity)
    {
        entity.ToTable("FgsSetupPricingMatrixLaborTier");
        entity.HasKey(e => e.Id);
        entity.ConfigureTenantCompanyGuidSetupColumns();
        entity.ConfigureTenantCompanyGuidSetupFk("FK_FgsSetupPricingMatrixLaborTier_Company");
        entity.Property(e => e.Rate).HasPrecision(18, 2);
        entity.HasOne<FgsSetupPricingMatrixLabor>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupPricingMatrixLaborId)
            .HasConstraintName("FK_FgsSetupPricingMatrixLaborTier_Labor")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => e.FgsSetupPricingMatrixLaborId);
    }
}
