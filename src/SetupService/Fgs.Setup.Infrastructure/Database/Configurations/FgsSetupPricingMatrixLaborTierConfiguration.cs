using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPricingMatrixLaborTierConfiguration : IEntityTypeConfiguration<FgsSetupPricingMatrixLaborTier>
{
    public void Configure(EntityTypeBuilder<FgsSetupPricingMatrixLaborTier> entity)
    {
        entity.ToTable(
            "FgsSetupPricingMatrixLaborTier",
            t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixLaborTier_DurationMinutes",
                    "\"DurationMinutes\" > 0");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixLaborTier_Rate",
                    "\"Rate\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.Rate).HasPrecision(18, 2);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasAlternateKey(e => new
        {
            e.TenantId,
            e.CompanyId,
            e.PricingMatrixLaborId,
            e.SequenceOrder,
        })
            .HasName("UQ_FgsSetupPricingMatrixLaborTier_PricingMatrixLaborId_SequenceOrder");

        entity.HasOne<FgsSetupPricingMatrixLabor>()
            .WithMany()
            .HasForeignKey(e => e.PricingMatrixLaborId)
            .HasConstraintName("FK_FgsSetupPricingMatrixLaborTier_PricingMatrixLabor")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PricingMatrixLaborId })
            .HasDatabaseName("IX_FgsSetupPricingMatrixLaborTier_TenantId_CompanyId_PricingMatrixLaborId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSetupPricingMatrixLaborTier_TenantId_CompanyId_IsActive");
    }
}
