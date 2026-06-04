using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixLaborTierConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrixLaborTier>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrixLaborTier> entity)
    {
        entity.ToTable("FgsSetupPricingMatrixLaborTier");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn().HasColumnType("integer");
        entity.ConfigureTenantCompanySetupColumns();
        entity.Property(e => e.Rate).HasPrecision(18, 2);
        entity.HasOne<FgsSetupPricingMatrixLabor>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupPricingMatrixLaborId)
            .HasConstraintName("FK_FgsSetupPricingMatrixLaborTier_Labor")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => e.FgsSetupPricingMatrixLaborId);
    }
}
