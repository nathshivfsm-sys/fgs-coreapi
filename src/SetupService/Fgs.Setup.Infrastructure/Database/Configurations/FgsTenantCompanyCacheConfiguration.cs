using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class FgsTenantCompanyCacheConfiguration : IEntityTypeConfiguration<FgsTenantCompanyCache>
{
    public void Configure(EntityTypeBuilder<FgsTenantCompanyCache> entity)
    {
        entity.ToTable(
            "FgsTenantCompanyCache",
            t => t.HasComment(
                "Local cache of tenant company identity information used for CompanyGuid resolution and elimination of cross-schema dependencies."));

        entity.HasKey(e => new { e.TenantId, e.CompanyId })
            .HasName("PK_FgsTenantCompanyCache");

        entity.Property(e => e.TenantId).HasComment("Identifier of the tenant that owns the company.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier used throughout FSM. Maps to tenant.FgsTenantCompany.CompanyNumber.");
        entity.Property(e => e.CompanyGuid).HasComment("Globally unique public identifier used by external integrations and APIs.");
        entity.Property(e => e.Code).HasMaxLength(100).HasComment("Human-readable company code.");
        entity.Property(e => e.Name).HasMaxLength(200).HasComment("Display name of the company.");
        entity.Property(e => e.IsActive).HasComment("Indicates whether the company is active.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Timestamp of the most recent synchronization from tenant.FgsTenantCompany.");

        entity.HasIndex(e => e.CompanyGuid)
            .IsUnique()
            .HasDatabaseName("UX_FgsTenantCompanyCache_CompanyGuid");

        entity.HasIndex(e => new { e.TenantId, e.Code })
            .HasDatabaseName("IX_FgsTenantCompanyCache_TenantId_Code");

        entity.HasIndex(e => new { e.TenantId, e.Name })
            .HasDatabaseName("IX_FgsTenantCompanyCache_TenantId_Name");
    }
}
