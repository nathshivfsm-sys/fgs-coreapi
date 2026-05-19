using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixOtherConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrixOther>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrixOther> entity)
    {
        entity.ToTable("FgsSetupPricingMatrixOther");
        entity.HasKey(e => e.Id);
        entity.ConfigureTenantCompanyGuidSetupColumns();
        entity.ConfigureTenantCompanyGuidSetupFk("FK_FgsSetupPricingMatrixOther_Company");
        entity.Property(e => e.MarkupPercent).HasPrecision(18, 2);
        entity.Property(e => e.DiscountPercent).HasPrecision(18, 2);
        entity.HasOne<FgsSetupPricingMatrix>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupPricingMatrixId)
            .HasConstraintName("FK_FgsSetupPricingMatrixOther_PricingMatrix")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.FgsSetupPricingMatrixId, e.CategoryCode })
            .HasName("UQ_FgsSetupPricingMatrixOther");
        entity.HasIndex(e => e.FgsSetupPricingMatrixId);
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixOther_MarkupPercent",
                "\"MarkupPercent\" IS NULL OR \"MarkupPercent\" >= 0");
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixOther_DiscountPercent",
                "\"DiscountPercent\" IS NULL OR (\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100)");
        });
    }
}
