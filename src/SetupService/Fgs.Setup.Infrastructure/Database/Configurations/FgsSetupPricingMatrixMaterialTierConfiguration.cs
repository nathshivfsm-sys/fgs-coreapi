using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixMaterialTierConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrixMaterialTier>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrixMaterialTier> entity)
    {
        entity.ToTable(
            "FgsSetupPricingMatrixMaterialTier",
            t => t.HasComment(
                "Defines material cost tiers and pricing adjustments used by a pricing matrix. Each tier applies a single pricing adjustment method to determine the selling price from material cost."));
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn().HasColumnType("integer");
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.Property(e => e.FgsSetupPricingMatrixId)
            .HasComment("Reference to the pricing matrix that contains this tier.");
        entity.Property(e => e.FromCost)
            .HasPrecision(18, 2)
            .HasComment("Inclusive minimum material cost for this pricing tier.");
        entity.Property(e => e.ToCost)
            .HasPrecision(18, 2)
            .HasComment("Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit.");
        entity.Property(e => e.PriceAdjustmentTypeId)
            .HasColumnType("smallint")
            .HasComment("Pricing adjustment method. Valid values: 1=Markup Percent, 2=Markup Amount, 3=Multiplier.");
        entity.Property(e => e.AdjustmentValue)
            .HasPrecision(18, 6)
            .HasComment("Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = $150 markup, 1.75 = multiplier.");
        entity.HasOne<FgsSetupPricingMatrix>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupPricingMatrixId)
            .HasConstraintName("FK_FgsSetupPricingMatrixMaterialTier_PricingMatrix")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupPricingMatrixId })
            .HasDatabaseName("IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId_FgsSetupPricingMatrixId");
        entity.HasIndex(e => e.PriceAdjustmentTypeId)
            .HasDatabaseName("IX_FgsSetupPricingMatrixMaterialTier_PriceAdjustmentTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupPricingMatrixId, e.FromCost })
            .IsUnique()
            .HasDatabaseName("UQ_FgsSetupPricingMatrixMaterialTier_Matrix_FromCost");
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixMaterialTier_FromCost",
                "\"FromCost\" >= 0");
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixMaterialTier_ToCost",
                "\"ToCost\" IS NULL OR \"ToCost\" >= \"FromCost\"");
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixMaterialTier_PriceAdjustmentTypeId",
                "\"PriceAdjustmentTypeId\" BETWEEN 1 AND 3");
            t.HasCheckConstraint(
                "CK_FgsSetupPricingMatrixMaterialTier_AdjustmentValue",
                "\"AdjustmentValue\" >= 0");
        });
    }
}
