using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateStatusConfiguration : IEntityTypeConfiguration<FgsEstimateStatus>
{
    public void Configure(EntityTypeBuilder<FgsEstimateStatus> entity)
    {
        entity.ToTable(
            "FgsEstimateStatus",
            t => t.HasComment(
                "Stores estimate statuses available to a specific tenant/company. StatusCode is immutable and used by application business logic."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.StatusCode).HasMaxLength(50).IsRequired()
            .HasComment("Immutable system status code used by application business logic.");
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired()
            .HasComment("User-facing display name that may be customized by the tenant.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.StatusCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateStatus_TenantId_CompanyId_StatusCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateStatus_TenantId_CompanyId_Name");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateStatus_TenantId_CompanyId");
    }
}
