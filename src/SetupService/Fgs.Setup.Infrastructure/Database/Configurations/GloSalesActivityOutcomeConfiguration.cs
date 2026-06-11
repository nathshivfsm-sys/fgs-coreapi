using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloSalesActivityOutcomeConfiguration : IEntityTypeConfiguration<GloSalesActivityOutcome>
{
    public void Configure(EntityTypeBuilder<GloSalesActivityOutcome> entity)
    {
        entity.ToTable(
            "GloSalesActivityOutcome",
            t =>
            {
                t.HasComment(
                    "Master list of sales activity outcomes used by Leads and Opportunities. Outcomes represent the result of a sales interaction and may optionally suggest the next sales pipeline status. Seeded into setup.FgsSalesActivityOutcome.");
                t.HasCheckConstraint(
                    "CK_GloSalesActivityOutcome_AppliesToEntity",
                    "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the sales activity outcome.");

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
        entity.Property(e => e.NextSalesPipelineStatusCode)
            .HasMaxLength(50)
            .HasComment("Suggested sales pipeline status code that should be applied when this outcome is selected.");
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

        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");

        entity.HasIndex(e => e.OutcomeCode)
            .IsUnique()
            .HasDatabaseName("UX_GloSalesActivityOutcome_OutcomeCode");

        entity.HasIndex(e => e.OutcomeName)
            .IsUnique()
            .HasDatabaseName("UX_GloSalesActivityOutcome_OutcomeName");

        entity.HasIndex(e => e.DisplayOrder)
            .HasDatabaseName("IX_GloSalesActivityOutcome_DisplayOrder");
    }
}
