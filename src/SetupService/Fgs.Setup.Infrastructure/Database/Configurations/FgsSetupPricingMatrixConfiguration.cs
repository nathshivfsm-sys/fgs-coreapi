using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrix>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrix> entity)
    {
        entity.ToTable("FgsSetupPricingMatrix");
        entity.HasKey(e => e.Id);
        entity.ConfigureTenantCompanyGuidSetupColumns();
        entity.Property(e => e.EffectiveFrom).HasColumnType("date");
        entity.Property(e => e.EffectiveTo).HasColumnType("date");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupPricingMatrix");
    }
}
