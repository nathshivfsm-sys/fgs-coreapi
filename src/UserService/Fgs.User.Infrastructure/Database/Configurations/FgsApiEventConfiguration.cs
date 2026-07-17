using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsApiEventConfiguration : IEntityTypeConfiguration<FgsApiEvent>
{
    public void Configure(EntityTypeBuilder<FgsApiEvent> entity)
    {
        entity.ToTable(
            "FgsApiEvent",
            t => t.HasComment(
                "Master catalog of public API events that external applications may subscribe to through webhooks."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();

        entity.Property(e => e.EventCode)
            .HasMaxLength(100)
            .HasComment("Unique event identifier exposed through the public API. Example: workorder.completed.");
        entity.Property(e => e.EventCategory)
            .HasMaxLength(50)
            .HasComment("Logical category used to organize events, such as WorkOrder, Estimate, Invoice, Customer or Payment.");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasComment("Display name of the API event.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Description of when the event is published.");
        entity.Property(e => e.EventVersion)
            .HasDefaultValue((short)1)
            .HasComment("Version number of the public event contract. Used to support backward-compatible changes to webhook payloads and API event schemas.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the display order within the Developer Portal.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the event is available for webhook subscriptions.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the API event was created.");

        entity.HasIndex(e => e.EventCode)
            .IsUnique()
            .HasDatabaseName("IX_FgsApiEvent_EventCode");
        entity.HasIndex(e => e.EventCategory)
            .HasDatabaseName("IX_FgsApiEvent_EventCategory");
    }
}
