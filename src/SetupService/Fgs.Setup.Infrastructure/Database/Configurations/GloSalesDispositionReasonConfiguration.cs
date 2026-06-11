using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloSalesDispositionReasonConfiguration : IEntityTypeConfiguration<GloSalesDispositionReason>
{
    public void Configure(EntityTypeBuilder<GloSalesDispositionReason> entity)
    {
        entity.ToTable(
            "GloSalesDispositionReason",
            t =>
            {
                t.HasComment(
                    "Master list of sales disposition reasons used when a Lead is Disqualified or an Opportunity is Lost. Seeded into setup.FgsSalesDispositionReason.");
                t.HasCheckConstraint(
                    "CK_GloSalesDispositionReason_AppliesToEntity",
                    "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the sales disposition reason.");

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
            .HasComment("Indicates whether selecting this disposition reason should result in a terminal pipeline status such as Lost or Disqualified.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which reasons are displayed.");

        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");

        entity.HasIndex(e => e.DispositionReasonCode)
            .IsUnique()
            .HasDatabaseName("UX_GloSalesDispositionReason_DispositionReasonCode");

        entity.HasIndex(e => e.DispositionReasonName)
            .IsUnique()
            .HasDatabaseName("UX_GloSalesDispositionReason_DispositionReasonName");

        entity.HasIndex(e => e.DisplayOrder)
            .HasDatabaseName("IX_GloSalesDispositionReason_DisplayOrder");
    }
}
