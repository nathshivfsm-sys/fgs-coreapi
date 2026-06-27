using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsAppointmentAssignmentEventConfiguration : IEntityTypeConfiguration<FgsAppointmentAssignmentEvent>
{
    public void Configure(EntityTypeBuilder<FgsAppointmentAssignmentEvent> entity)
    {
        entity.ToTable(
            "FgsAppointmentAssignmentEvent",
            t => t.HasComment(
                "Stores technician activity events used for dispatch tracking, payroll calculations, utilization reporting and technician history."));

        entity.HasKey(e => e.Id).HasName("PK_FgsAppointmentAssignmentEvent");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.AssignmentId).HasComment("Appointment assignment associated with the event. NULL for technician-only events such as On Duty, Off Duty, Lunch Start and Lunch End.");
        entity.Property(e => e.EmployeeId).HasComment("Employee associated with the event. References user service; no FK by design.");
        entity.Property(e => e.ServiceDate).HasColumnType("date").IsRequired()
            .HasComment("Business service date associated with the event. Used for overnight work and payroll calculations.");
        entity.Property(e => e.EventTypeId).HasComment("References glo.GloAppointmentAssignmentEventType.EventTypeId.");
        entity.Property(e => e.EventOccurredOn).IsRequired().HasColumnType("timestamptz")
            .HasComment("Actual timestamp when the event occurred.");
        entity.Property(e => e.EnteredByOffice).HasDefaultValue(false).IsRequired()
            .HasComment("Indicates the event was entered or reconstructed by office staff rather than captured by the technician.");
        entity.Property(e => e.Notes).HasMaxLength(500).HasComment("Optional notes entered by office staff or technician.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100).HasComment("User who last updated the record.");

        entity.HasOne<FgsAppointmentAssignment>()
            .WithMany()
            .HasForeignKey(e => e.AssignmentId)
            .HasConstraintName("FK_FgsAppointmentAssignmentEvent_FgsAppointmentAssignment")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsAppointmentAssignmentEvent_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssignmentId }).HasDatabaseName("IX_FgsAppointmentAssignmentEvent_Assignment");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssignmentId, e.EventOccurredOn })
            .HasDatabaseName("IX_FgsAppointmentAssignmentEvent_AssignmentEventOccurredOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId }).HasDatabaseName("IX_FgsAppointmentAssignmentEvent_Employee");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId, e.EventOccurredOn })
            .HasDatabaseName("IX_FgsAppointmentAssignmentEvent_EmployeeEventOccurredOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceDate }).HasDatabaseName("IX_FgsAppointmentAssignmentEvent_ServiceDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EventTypeId }).HasDatabaseName("IX_FgsAppointmentAssignmentEvent_EventType");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId, e.EventTypeId, e.EventOccurredOn })
            .IsUnique()
            .HasDatabaseName("UX_FgsAppointmentAssignmentEvent_NoDuplicates");
    }
}
