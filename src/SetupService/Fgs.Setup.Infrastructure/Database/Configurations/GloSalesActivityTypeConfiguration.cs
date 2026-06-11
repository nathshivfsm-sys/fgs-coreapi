using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloSalesActivityTypeConfiguration : IEntityTypeConfiguration<GloSalesActivityType>
{
    public void Configure(EntityTypeBuilder<GloSalesActivityType> entity)
    {
        entity.ToTable(
            "GloSalesActivityType",
            t =>
            {
                t.HasComment(
                    "Master list of sales activity types used by Leads and Opportunities. Seeded into setup.FgsSalesActivityType.");
                t.HasCheckConstraint(
                    "CK_GloSalesActivityType_AppliesToEntity",
                    "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the sales activity type.");

        entity.Property(e => e.ActivityTypeCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Immutable business code for the sales activity type.");
        entity.Property(e => e.ActivityTypeName)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("User-friendly name displayed throughout the application.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the sales activity type.");
        entity.Property(e => e.AppliesToLead)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the activity type can be used by Leads.");
        entity.Property(e => e.AppliesToOpportunity)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the activity type can be used by Opportunities.");
        entity.Property(e => e.AllowManualSelection)
            .HasDefaultValue(true)
            .HasComment("Indicates whether users may manually select this activity type.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which activity types are displayed.");

        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");

        entity.HasIndex(e => e.ActivityTypeCode)
            .IsUnique()
            .HasDatabaseName("UX_GloSalesActivityType_ActivityTypeCode");

        entity.HasIndex(e => e.ActivityTypeName)
            .IsUnique()
            .HasDatabaseName("UX_GloSalesActivityType_ActivityTypeName");

        entity.HasIndex(e => e.DisplayOrder)
            .HasDatabaseName("IX_GloSalesActivityType_DisplayOrder");
    }
}
