using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixOtherConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrixOther>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrixOther> entity)
    {
        entity.ToTable(
            "FgsSetupPricingMatrixOther",
            t =>
            {
                t.HasComment(
                    "Stores pricing adjustments for miscellaneous categories such as permits, disposal fees, equipment rentals, trip charges, crane services, and other non-material/non-labor costs.");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixOther_MarkupPercent",
                    "\"MarkupPercent\" IS NULL OR \"MarkupPercent\" >= 0");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixOther_DiscountPercent",
                    "\"DiscountPercent\" IS NULL OR (\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100)");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.PricingMatrixId)
            .HasComment("Reference to the pricing matrix.");
        entity.Property(e => e.CategoryCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Unique category code within the pricing matrix.");
        entity.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("User-friendly category name.");
        entity.Property(e => e.MarkupPercent)
            .HasPrecision(18, 2)
            .HasComment("Markup percentage applied to the base cost.");
        entity.Property(e => e.DiscountPercent)
            .HasPrecision(18, 2)
            .HasComment("Optional discount percentage applied after markup.");
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.PricingMatrixId, e.CategoryCode })
            .HasName("UQ_FgsSetupPricingMatrixOther");

        entity.HasOne<FgsSetupPricingMatrix>()
            .WithMany()
            .HasForeignKey(e => e.PricingMatrixId)
            .HasConstraintName("FK_FgsSetupPricingMatrixOther_PricingMatrix")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PricingMatrixId })
            .HasDatabaseName("IX_FgsSetupPricingMatrixOther_TenantId_CompanyId_PricingMatrixId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSetupPricingMatrixOther_TenantId_CompanyId_IsActive");
    }
}
