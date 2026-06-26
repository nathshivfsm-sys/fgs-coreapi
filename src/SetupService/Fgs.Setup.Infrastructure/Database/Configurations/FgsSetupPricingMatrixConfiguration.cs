using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrix>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrix> entity)
    {
        entity.ToTable(
            "FgsSetupPricingMatrix",
            t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrix_EffectiveDates",
                    "\"EffectiveTo\" IS NULL OR \"EffectiveTo\" >= \"EffectiveFrom\"");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.IsLaborTierStructure).HasDefaultValue(false);
        entity.Property(e => e.IsLaborRateBySkillLevel).HasDefaultValue(false);
        entity.Property(e => e.EffectiveFrom).HasColumnType("date");
        entity.Property(e => e.EffectiveTo).HasColumnType("date");
        entity.Property(e => e.IsMobileVisible).HasDefaultValue(true);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupPricingMatrix_TenantId_CompanyId_Code");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsSetupPricingMatrix_TenantId_CompanyId_Name");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSetupPricingMatrix_TenantId_CompanyId_IsActive");
    }
}
