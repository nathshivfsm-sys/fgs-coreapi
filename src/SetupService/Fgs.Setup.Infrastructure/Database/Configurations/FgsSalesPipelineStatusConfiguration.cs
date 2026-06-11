using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSalesPipelineStatusConfiguration : IEntityTypeConfiguration<FgsSalesPipelineStatus>
{
    public void Configure(EntityTypeBuilder<FgsSalesPipelineStatus> entity)
    {
        entity.ToTable(
            "FgsSalesPipelineStatus",
            t =>
            {
                t.HasComment(
                    "Stores tenant/company specific sales pipeline statuses used by Leads and Opportunities. Seeded from glo.GloSalesPipelineStatus.");
                t.HasCheckConstraint(
                    "CK_FgsSalesPipelineStatus_AppliesToEntity",
                    "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn()
            .HasComment("Unique identifier for the sales pipeline status.");

        entity.Property(e => e.TenantId).HasComment("Tenant identifier that owns the record.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier that owns the record.");
        entity.Property(e => e.StatusCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Immutable business code for the sales pipeline status.");
        entity.Property(e => e.StatusName)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("User-friendly name displayed throughout the application.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the purpose of the status.");
        entity.Property(e => e.AppliesToLead)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the status can be used by Leads.");
        entity.Property(e => e.AppliesToOpportunity)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the status can be used by Opportunities.");
        entity.Property(e => e.IsTerminal)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified.");
        entity.Property(e => e.AllowManualSelection)
            .HasDefaultValue(true)
            .HasComment("Indicates whether users may manually select this status.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which statuses are displayed.");
        entity.Property(e => e.IsSystem)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the status was seeded by the system. System records should have immutable business codes.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the status is available for use.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsSalesPipelineStatus_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.StatusCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsSalesPipelineStatus_TenantId_CompanyId_StatusCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.StatusName })
            .IsUnique()
            .HasDatabaseName("UX_FgsSalesPipelineStatus_TenantId_CompanyId_StatusName");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsSalesPipelineStatus_TenantId_CompanyId_DisplayOrder");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSalesPipelineStatus_TenantId_CompanyId_IsActive");
    }
}
