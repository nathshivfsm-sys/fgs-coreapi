using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSalesActivityOutcomeConfiguration : IEntityTypeConfiguration<FgsSalesActivityOutcome>
{
    public void Configure(EntityTypeBuilder<FgsSalesActivityOutcome> entity)
    {
        entity.ToTable(
            "FgsSalesActivityOutcome",
            t =>
            {
                t.HasComment(
                    "Stores tenant/company specific sales activity outcomes used by Leads and Opportunities. Outcomes represent the result of a sales interaction and may optionally suggest the next sales pipeline status. Seeded from glo.GloSalesActivityOutcome.");
                t.HasCheckConstraint(
                    "CK_FgsSalesActivityOutcome_AppliesToEntity",
                    "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn()
            .HasComment("Unique identifier for the sales activity outcome.");

        entity.Property(e => e.TenantId).HasComment("Tenant identifier that owns the record.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier that owns the record.");
        entity.Property(e => e.OutcomeCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Immutable business code for the sales activity outcome.");
        entity.Property(e => e.OutcomeName)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("User-friendly name displayed throughout the application.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the sales activity outcome.");
        entity.Property(e => e.AppliesToLead)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the outcome can be used by Leads.");
        entity.Property(e => e.AppliesToOpportunity)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the outcome can be used by Opportunities.");
        entity.Property(e => e.NextSalesPipelineStatusId)
            .HasComment("Suggested sales pipeline status that should be applied when this outcome is selected.");
        entity.Property(e => e.IsTerminal)
            .HasDefaultValue(false)
            .HasComment("Indicates whether selecting this outcome typically results in a terminal sales pipeline status.");
        entity.Property(e => e.RequireComment)
            .HasDefaultValue(false)
            .HasComment("Indicates whether users must provide additional comments when selecting this outcome.");
        entity.Property(e => e.AllowManualSelection)
            .HasDefaultValue(true)
            .HasComment("Indicates whether users may manually select this outcome.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which outcomes are displayed.");
        entity.Property(e => e.IsSystem)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the outcome was seeded by the system. System records should have immutable business codes.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the outcome is available for use.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<FgsSalesPipelineStatus>()
            .WithMany()
            .HasForeignKey(e => e.NextSalesPipelineStatusId)
            .HasConstraintName("FK_FgsSalesActivityOutcome_FgsSalesPipelineStatus_NextSalesPipelineStatusId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsSalesActivityOutcome_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.OutcomeCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsSalesActivityOutcome_TenantId_CompanyId_OutcomeCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.OutcomeName })
            .IsUnique()
            .HasDatabaseName("UX_FgsSalesActivityOutcome_TenantId_CompanyId_OutcomeName");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsSalesActivityOutcome_TenantId_CompanyId_DisplayOrder");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSalesActivityOutcome_TenantId_CompanyId_IsActive");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.NextSalesPipelineStatusId })
            .HasDatabaseName("IX_FgsSalesActivityOutcome_TenantId_CompanyId_NextStatusId");
    }
}
