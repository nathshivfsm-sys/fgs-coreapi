using Fgs.Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Audit.Infrastructure.Database.Configurations;

internal sealed class FgsEventDetailConfiguration : IEntityTypeConfiguration<FgsEventDetail>
{
    public void Configure(EntityTypeBuilder<FgsEventDetail> entity)
    {
        entity.ToTable(
            "FgsEventDetail",
            t => t.HasComment(
                "Stores detailed information associated with an event, including field changes, calculations, validation results, workflow actions, messages, and exceptions."));

        entity.HasKey(e => e.Id).HasName("PK_FgsEventDetail");
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasIdentityOptions(startValue: 1, incrementBy: 1)
            .HasComment("Unique identifier of the event detail record.");

        entity.Property(e => e.EventId).IsRequired()
            .HasComment("References the parent event.");
        entity.Property(e => e.EntryType).IsRequired()
            .HasComment("Classifies the type of detail entry, such as field change, calculation, validation, message, or exception.");
        entity.Property(e => e.Sequence)
            .HasDefaultValue((short)1)
            .IsRequired()
            .HasComment("Determines the display order of detail entries within an event.");
        entity.Property(e => e.ItemName).HasMaxLength(100).IsRequired()
            .HasComment("Name of the property, calculation item, validation rule, or message attribute.");
        entity.Property(e => e.OldValue).HasColumnType("text")
            .HasComment("Original value before the event occurred. Typically populated for field changes.");
        entity.Property(e => e.NewValue).HasColumnType("text")
            .HasComment("New value after the event occurred, or the resulting value for calculations, messages, and other detail types.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .IsRequired()
            .HasComment("Date and time the detail record was created.");

        entity.HasIndex(e => e.EventId)
            .HasDatabaseName("IX_FgsEventDetail_EventId");
        entity.HasIndex(e => new { e.EventId, e.Sequence })
            .HasDatabaseName("IX_FgsEventDetail_EventId_Sequence");
        entity.HasIndex(e => e.EntryType)
            .HasDatabaseName("IX_FgsEventDetail_EntryType");
    }
}
