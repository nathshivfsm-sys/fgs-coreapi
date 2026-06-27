using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateFlavorConfiguration : IEntityTypeConfiguration<FgsEstimateFlavor>
{
    public void Configure(EntityTypeBuilder<FgsEstimateFlavor> entity)
    {
        entity.ToTable(
            "FgsEstimateFlavor",
            t => t.HasComment(
                "Stores estimate flavor definitions used to visually categorize estimate options such as Good, Better, Best, Popular, Premium, Bronze, Silver, and Gold."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.FlavorCode).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        entity.Property(e => e.BackgroundColor).HasMaxLength(20).IsRequired();
        entity.Property(e => e.TextColor).HasMaxLength(20).IsRequired();
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FlavorCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateFlavor_TenantId_CompanyId_FlavorCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateFlavor_TenantId_CompanyId");
    }
}
