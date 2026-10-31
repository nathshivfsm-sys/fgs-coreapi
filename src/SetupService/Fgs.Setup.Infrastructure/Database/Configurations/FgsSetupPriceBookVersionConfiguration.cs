using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPriceBookVersionConfiguration : IEntityTypeConfiguration<FgsSetupPriceBookVersion>
{
    public void Configure(EntityTypeBuilder<FgsSetupPriceBookVersion> entity)
    {
        entity.ToTable(
            "FgsSetupPriceBookVersion",
            t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupPriceBookVersion_EffectiveDates",
                    "\"EffectiveTo\" IS NULL OR \"EffectiveTo\" >= \"EffectiveFrom\"");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.Version).HasMaxLength(25).IsRequired();
        entity.Property(e => e.EffectiveFrom).HasColumnType("date");
        entity.Property(e => e.EffectiveTo).HasColumnType("date");
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Version })
            .IsUnique()
            .HasDatabaseName("UQ_FgsSetupPriceBookVersion_TenantId_CompanyId_Version");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSetupPriceBookVersion_TenantId_CompanyId_IsActive");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EffectiveFrom })
            .HasDatabaseName("IX_FgsSetupPriceBookVersion_TenantId_CompanyId_EffectiveFrom");
    }
}
