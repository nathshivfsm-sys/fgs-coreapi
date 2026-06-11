using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSalesDispositionReasonConfiguration : IEntityTypeConfiguration<FgsSalesDispositionReason>
{
    public void Configure(EntityTypeBuilder<FgsSalesDispositionReason> entity)
    {
        entity.ToTable(
            "FgsSalesDispositionReason",
            t =>
            {
                t.HasComment(
                    "Stores tenant/company specific sales disposition reasons used when a Lead is Disqualified or an Opportunity is Lost. Seeded from glo.GloSalesDispositionReason.");
                t.HasCheckConstraint(
                    "CK_FgsSalesDispositionReason_AppliesToEntity",
                    "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn()
            .HasComment("Unique identifier for the sales disposition reason.");

        entity.Property(e => e.TenantId).HasComment("Tenant identifier that owns the record.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier that owns the record.");
        entity.Property(e => e.DispositionReasonCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Immutable business code for the disposition reason.");
        entity.Property(e => e.DispositionReasonName)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("User-friendly name displayed throughout the application.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the disposition reason.");
        entity.Property(e => e.AppliesToLead)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the reason can be used when a Lead is Disqualified.");
        entity.Property(e => e.AppliesToOpportunity)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the reason can be used when an Opportunity is Lost.");
        entity.Property(e => e.RequireComment)
            .HasDefaultValue(false)
            .HasComment("Indicates whether users must provide additional comments when selecting this disposition reason.");
        entity.Property(e => e.IsTerminal)
            .HasDefaultValue(true)
            .HasComment("Indicates whether selecting this disposition reason should result in a terminal pipeline status.");
        entity.Property(e => e.AllowManualSelection)
            .HasDefaultValue(true)
            .HasComment("Indicates whether users may manually select this disposition reason.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which disposition reasons are displayed.");
        entity.Property(e => e.IsSystem)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the disposition reason was seeded by the system. System records should have immutable business codes.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the disposition reason is available for use.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsSalesDispositionReason_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DispositionReasonCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsSalesDispReason_TenantId_CompanyId_ReasonCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DispositionReasonName })
            .IsUnique()
            .HasDatabaseName("UX_FgsSalesDispReason_TenantId_CompanyId_ReasonName");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsSalesDispositionReason_TenantId_CompanyId_DisplayOrder");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSalesDispositionReason_TenantId_CompanyId_IsActive");
    }
}
