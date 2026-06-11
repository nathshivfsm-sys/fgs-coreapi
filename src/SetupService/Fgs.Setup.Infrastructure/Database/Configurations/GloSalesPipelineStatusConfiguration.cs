using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloSalesPipelineStatusConfiguration : IEntityTypeConfiguration<GloSalesPipelineStatus>
{
    public void Configure(EntityTypeBuilder<GloSalesPipelineStatus> entity)
    {
        entity.ToTable(
            "GloSalesPipelineStatus",
            t =>
            {
                t.HasComment(
                    "Master list of sales pipeline statuses used by Leads and Opportunities. Seeded into setup.FgsSalesPipelineStatus.");
                t.HasCheckConstraint(
                    "CK_GloSalesPipelineStatus_AppliesToEntity",
                    "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the sales pipeline status.");

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
            .HasComment("Indicates whether users may manually select this status. When false, the status should be reached through workflow actions or automation.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which statuses are displayed.");

        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");

        entity.HasIndex(e => e.StatusCode)
            .IsUnique()
            .HasDatabaseName("UX_GloSalesPipelineStatus_StatusCode");

        entity.HasIndex(e => e.StatusName)
            .IsUnique()
            .HasDatabaseName("UX_GloSalesPipelineStatus_StatusName");

        entity.HasIndex(e => e.DisplayOrder)
            .HasDatabaseName("IX_GloSalesPipelineStatus_DisplayOrder");
    }
}
