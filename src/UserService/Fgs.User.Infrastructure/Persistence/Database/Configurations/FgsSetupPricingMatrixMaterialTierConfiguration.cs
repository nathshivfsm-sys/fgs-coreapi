using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsSetupPricingMatrixMaterialTierConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrixMaterialTier>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrixMaterialTier> entity)
    {
        entity.ToTable("FgsSetupPricingMatrixMaterialTier");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn().HasColumnType("integer");
        entity.ConfigureTenantCompanySetupColumns();
        entity.ConfigureTenantCompanySetupFk("FK_FgsSetupPricingMatrixMaterialTier_Company");
        entity.Property(e => e.FromCost).HasPrecision(18, 2);
        entity.Property(e => e.ToCost).HasPrecision(18, 2);
        entity.Property(e => e.MarkupPercent).HasPrecision(18, 2);
        entity.Property(e => e.DiscountPercent).HasPrecision(18, 2);
        entity.HasOne<FgsSetupPricingMatrix>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupPricingMatrixId)
            .HasConstraintName("FK_FgsSetupPricingMatrixMaterialTier_PricingMatrix")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => e.FgsSetupPricingMatrixId);
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixMaterialTier_FromCost",
                "\"FromCost\" >= 0");
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixMaterialTier_ToCost",
                "\"ToCost\" IS NULL OR \"ToCost\" >= \"FromCost\"");
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixMaterialTier_MarkupPercent",
                "\"MarkupPercent\" >= 0");
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixMaterialTier_DiscountPercent",
                "\"DiscountPercent\" IS NULL OR (\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100)");
        });
    }
}
