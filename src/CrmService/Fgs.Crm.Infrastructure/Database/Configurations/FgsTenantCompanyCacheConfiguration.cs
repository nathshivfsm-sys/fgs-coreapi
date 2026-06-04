using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsTenantCompanyCacheConfiguration : IEntityTypeConfiguration<FgsTenantCompanyCache>
{
    public void Configure(EntityTypeBuilder<FgsTenantCompanyCache> entity)
    {
        entity.ToTable(
            "FgsTenantCompanyCache",
            t => t.HasComment(
                "Local cache of tenant company information used by the CRM schema to eliminate cross-schema dependencies on tenant.FgsTenantCompany."));

        entity.HasKey(e => new { e.TenantId, e.CompanyId }).HasName("PK_FgsTenantCompanyCache");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier mapped from tenant.FgsTenantCompany.CompanyNumber.");
        entity.Property(e => e.CompanyGuid).HasComment("Globally unique company identifier used by integrations and external systems.");
        entity.Property(e => e.CompanyCode).HasMaxLength(100).HasComment("Unique company code within a tenant.");
        entity.Property(e => e.CompanyName).HasMaxLength(200).HasComment("Display name of the company.");
        entity.Property(e => e.IsActive).HasComment("Indicates whether the company is active.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Timestamp of the most recent synchronization from tenant.FgsTenantCompany.");
        entity.HasIndex(e => e.CompanyGuid).IsUnique().HasDatabaseName("UQ_FgsTenantCompanyCache_CompanyGuid");
        entity.HasIndex(e => e.CompanyName).HasDatabaseName("IX_FgsTenantCompanyCache_CompanyName");
        entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_FgsTenantCompanyCache_IsActive");
    }
}
