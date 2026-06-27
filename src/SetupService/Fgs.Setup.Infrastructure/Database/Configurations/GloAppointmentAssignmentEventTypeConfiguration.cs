using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloAppointmentAssignmentEventTypeConfiguration : IEntityTypeConfiguration<GloAppointmentAssignmentEventType>
{
    public void Configure(EntityTypeBuilder<GloAppointmentAssignmentEventType> entity)
    {
        entity.ToTable(
            "GloAppointmentAssignmentEventType",
            t => t.HasComment(
                "Global catalog of technician appointment assignment event types used for dispatch tracking and payroll."));

        entity.HasKey(e => e.EventTypeId).HasName("PK_GloAppointmentAssignmentEventType");
        entity.Property(e => e.EventTypeId).HasColumnType("smallint").ValueGeneratedNever()
            .HasComment("Primary key and event type identifier referenced by dispatch.FgsAppointmentAssignmentEvent.");
        entity.Property(e => e.Code).HasMaxLength(50).IsRequired()
            .HasComment("Unique event type code.");
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired()
            .HasComment("Display name of the event type.");
        entity.Property(e => e.Description).HasMaxLength(255)
            .HasComment("Optional description of the event type.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Display order for UI and reporting.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the event type is active.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100).HasComment("User who last updated the record.");

        entity.HasIndex(e => e.Code).IsUnique().HasDatabaseName("UX_GloAppointmentAssignmentEventType_Code");
        entity.HasIndex(e => e.DisplayOrder).HasDatabaseName("IX_GloAppointmentAssignmentEventType_DisplayOrder");
    }
}
