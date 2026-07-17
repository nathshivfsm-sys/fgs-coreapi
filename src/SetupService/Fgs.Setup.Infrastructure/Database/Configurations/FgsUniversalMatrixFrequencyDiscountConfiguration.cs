using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsUniversalMatrixFrequencyDiscountConfiguration : IEntityTypeConfiguration<FgsUniversalMatrixFrequencyDiscount>
{
    public void Configure(EntityTypeBuilder<FgsUniversalMatrixFrequencyDiscount> entity)
    {
        entity.ToTable(
            "FgsUniversalMatrixFrequencyDiscount",
            t =>
            {
                t.HasComment("Stores company-specific service frequency options and their discount percentages.");
                t.HasCheckConstraint(
                    "CK_FgsUniversalMatrixFrequencyDiscount_DiscountPercent",
                    "\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: true);

        entity.Property(e => e.UniversalPricingServiceId)
            .HasComment("References the company-specific Universal Pricing Service configuration.");

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(e => e.DiscountPercent)
            .HasPrecision(8, 4)
            .HasDefaultValue(0m)
            .HasComment("Percentage discount applied based on the selected service frequency.");

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
            .HasConstraintName("FK_FgsUniversalMatrixFrequencyDiscount_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId })
            .HasDatabaseName("IX_FgsUniversalMatrixFrequencyDiscount_TenantId_CompanyId_UniversalPricingServiceId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_FgsUniversalMatrixFrequencyDiscount_TenantId_CompanyId_UniversalPricingServiceId_Name");
    }
}
