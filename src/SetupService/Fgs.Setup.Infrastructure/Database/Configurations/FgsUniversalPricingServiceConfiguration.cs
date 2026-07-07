using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsUniversalPricingServiceConfiguration : IEntityTypeConfiguration<FgsUniversalPricingService>
{
    public void Configure(EntityTypeBuilder<FgsUniversalPricingService> entity)
    {
        entity.ToTable("FgsUniversalPricingService", t =>
            t.HasComment("Defines Universal Pricing Services enabled and configured for a tenant company."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: true);

        entity.Property(e => e.UniversalPricingServiceCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Logical reference to glo.GloUniversalPricingService.ServiceCode. No cross-domain foreign key is enforced.");

        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the display sequence of the Universal Pricing Service for the company.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the Universal Pricing Service is currently active for the company.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Id })
            .HasName("AK_FgsUniversalPricingService_TenantId_CompanyId_Id");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UniversalPricingServiceCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsUniversalPricingService_TenantId_CompanyId_ServiceCode");
    }
}
