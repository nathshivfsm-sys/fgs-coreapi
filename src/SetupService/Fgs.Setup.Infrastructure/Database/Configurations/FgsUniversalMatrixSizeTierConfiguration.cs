using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsUniversalMatrixSizeTierConfiguration : IEntityTypeConfiguration<FgsUniversalMatrixSizeTier>
{
    public void Configure(EntityTypeBuilder<FgsUniversalMatrixSizeTier> entity)
    {
        entity.ToTable(
            "FgsUniversalMatrixSizeTier",
            t =>
            {
                t.HasComment("Stores company-specific size tiers and pricing multipliers for an enabled Universal Pricing Service.");
                t.HasCheckConstraint(
                    "CK_FgsUniversalMatrixSizeTier_Multiplier",
                    "\"Multiplier\" > 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: true);

        entity.Property(e => e.UniversalPricingServiceId)
            .HasComment("References the company-specific Universal Pricing Service configuration.");

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(e => e.Multiplier)
            .HasPrecision(8, 4)
            .HasDefaultValue(1.0000m)
            .HasComment("Company-specific multiplier applied for this size tier.");

        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1);

        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);

        entity.HasOne<FgsUniversalPricingService>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId })
            .HasPrincipalKey(p => new { p.TenantId, p.CompanyId, p.Id })
            .HasConstraintName("FK_FgsUniversalMatrixSizeTier_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId })
            .HasDatabaseName("IX_FgsUniversalMatrixSizeTier_TenantId_CompanyId_UniversalPricingServiceId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_FgsUniversalMatrixSizeTier_TenantId_CompanyId_UniversalPricingServiceId_Name");
    }
}
