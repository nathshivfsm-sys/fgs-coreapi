using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsUniversalMatrixItemConfiguration : IEntityTypeConfiguration<FgsUniversalMatrixItem>
{
    public void Configure(EntityTypeBuilder<FgsUniversalMatrixItem> entity)
    {
        entity.ToTable(
            "FgsUniversalMatrixItem",
            t =>
            {
                t.HasComment("Stores company-specific Universal Pricing Matrix items and base prices.");
                t.HasCheckConstraint(
                    "CK_FgsUniversalMatrixItem_BasePrice",
                    "\"BasePrice\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: true);

        entity.Property(e => e.UniversalPricingServiceId)
            .HasComment("References the company-specific Universal Pricing Service configuration.");

        entity.Property(e => e.ItemName)
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(e => e.UnitType)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Pricing unit used by the matrix item, such as Flat Rate, Sqft, Linear Foot, Window, or Bed.");

        entity.Property(e => e.BasePrice)
            .HasPrecision(18, 4)
            .HasDefaultValue(0m)
            .HasComment("Base price before tier, size, frequency, fee, add-on, tax, or other pricing adjustments.");

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
            .HasConstraintName("FK_FgsUniversalMatrixItem_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId })
            .HasDatabaseName("IX_FgsUniversalMatrixItem_TenantId_CompanyId_UniversalPricingServiceId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId, e.ItemName })
            .IsUnique()
            .HasDatabaseName("UX_FgsUniversalMatrixItem_TenantId_CompanyId_UniversalPricingServiceId_ItemName");
    }
}
