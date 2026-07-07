using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsUniversalMatrixOneTimeFeeConfiguration : IEntityTypeConfiguration<FgsUniversalMatrixOneTimeFee>
{
    public void Configure(EntityTypeBuilder<FgsUniversalMatrixOneTimeFee> entity)
    {
        entity.ToTable(
            "FgsUniversalMatrixOneTimeFee",
            t =>
            {
                t.HasComment("Stores company-specific one-time fees used by the Universal Pricing Matrix.");
                t.HasCheckConstraint(
                    "CK_FgsUniversalMatrixOneTimeFee_Amount",
                    "\"Amount\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: true);

        entity.Property(e => e.UniversalPricingServiceId)
            .HasComment("References the company-specific Universal Pricing Service configuration.");

        entity.Property(e => e.Name)
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(e => e.Amount)
            .HasPrecision(18, 4)
            .HasDefaultValue(0m)
            .HasComment("Fixed amount of the one-time fee.");

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
            .HasConstraintName("FK_FgsUniversalMatrixOneTimeFee_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId })
            .HasDatabaseName("IX_FgsUniversalMatrixOneTimeFee_TenantId_CompanyId_UniversalPricingServiceId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_FgsUniversalMatrixOneTimeFee_TenantId_CompanyId_UniversalPricingServiceId_Name");
    }
}
