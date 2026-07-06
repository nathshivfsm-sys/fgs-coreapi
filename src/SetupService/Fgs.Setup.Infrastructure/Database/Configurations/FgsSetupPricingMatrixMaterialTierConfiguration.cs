using Fgs.Setup.Domain.Entities;
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
            t =>
            {
                t.HasComment(
                    "Defines material cost tiers and pricing adjustments used by a pricing matrix. Each tier applies a single pricing adjustment method to determine the selling price from material cost.");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixMaterialTier_FromCost",
                    "\"FromCost\" >= 0");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixMaterialTier_ToCost",
                    "\"ToCost\" IS NULL OR \"ToCost\" >= \"FromCost\"");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixMaterialTier_AdjustmentValue",
                    "\"AdjustmentValue\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.PricingMatrixId)
            .HasComment("Reference to the pricing matrix that contains this tier.");
        entity.Property(e => e.FromCost)
            .HasPrecision(18, 2)
            .HasComment("Inclusive minimum material cost for this pricing tier.");
        entity.Property(e => e.ToCost)
            .HasPrecision(18, 2)
            .HasComment("Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit.");
        entity.Property(e => e.AdjustmentValue)
            .HasPrecision(18, 6)
            .HasDefaultValue(0m)
            .HasComment("Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = fixed dollar markup, 1.75 = multiplier.");
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasOne<FgsSetupPricingMatrix>()
            .WithMany()
            .HasForeignKey(e => e.PricingMatrixId)
            .HasConstraintName("FK_FgsSetupPricingMatrixMaterialTier_PricingMatrix")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PricingMatrixId })
            .HasDatabaseName("IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId_PricingMatrixId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PricingMatrixId, e.FromCost })
            .IsUnique()
            .HasDatabaseName("UQ_FgsSetupPricingMatrixMaterialTier_Matrix_FromCost");
    }
}
