using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsCrewConfiguration : IEntityTypeConfiguration<FgsCrew>
{
    public void Configure(EntityTypeBuilder<FgsCrew> entity)
    {
        entity.ToTable(
            "FgsCrew",
            t => t.HasComment(
                "Represents a technician crew used for scheduling, dispatching and workload management."));

        entity.HasKey(e => e.Id).HasName("PK_FgsCrew");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.CrewCode).HasMaxLength(25).IsRequired()
            .HasComment("Short unique crew code used on dispatch boards, reports and integrations.");
        entity.Property(e => e.CrewName).HasMaxLength(100).IsRequired()
            .HasComment("Display name of the crew.");
        entity.Property(e => e.Description).HasMaxLength(500).HasComment("Optional crew description.");
        entity.Property(e => e.IsActive).HasDefaultValue(true).IsRequired()
            .HasComment("Indicates whether the crew is available for scheduling and dispatching.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).IsRequired().HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Id })
            .HasName("UX_FgsCrew_TenantCompany_Id");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsCrew_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive }).HasDatabaseName("IX_FgsCrew_IsActive");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CrewCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsCrew_CrewCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CrewName })
            .IsUnique()
            .HasDatabaseName("UX_FgsCrew_CrewName");
    }
}
