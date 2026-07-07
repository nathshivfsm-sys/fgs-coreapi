using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsUniversalMatrixAddOnConfiguration : IEntityTypeConfiguration<FgsUniversalMatrixAddOn>
{
    public void Configure(EntityTypeBuilder<FgsUniversalMatrixAddOn> entity)
    {
        entity.ToTable(
            "FgsUniversalMatrixAddOn",
            t =>
            {
                t.HasComment("Stores company-specific optional add-ons available within the Universal Pricing Matrix.");
                t.HasCheckConstraint(
                    "CK_FgsUniversalMatrixAddOn_Price",
                    "\"Price\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: true);

        entity.Property(e => e.UniversalPricingServiceId)
            .HasComment("References the company-specific Universal Pricing Service configuration.");

        entity.Property(e => e.Name)
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(e => e.UnitType)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Pricing unit for the add-on, such as Flat Rate, Window, or Bed.");

        entity.Property(e => e.Price)
            .HasPrecision(18, 4)
            .HasDefaultValue(0m)
            .HasComment("Company-specific price per add-on pricing unit.");

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
            .HasConstraintName("FK_FgsUniversalMatrixAddOn_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId })
            .HasDatabaseName("IX_FgsUniversalMatrixAddOn_TenantId_CompanyId_UniversalPricingServiceId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_FgsUniversalMatrixAddOn_TenantId_CompanyId_UniversalPricingServiceId_Name");
    }
}
