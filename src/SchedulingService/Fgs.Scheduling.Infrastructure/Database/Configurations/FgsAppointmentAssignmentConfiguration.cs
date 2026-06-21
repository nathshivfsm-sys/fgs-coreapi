using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsAppointmentAssignmentConfiguration : IEntityTypeConfiguration<FgsAppointmentAssignment>
{
    public void Configure(EntityTypeBuilder<FgsAppointmentAssignment> entity)
    {
        entity.ToTable(
            "FgsAppointmentAssignment",
            t =>
            {
                t.HasComment("Represents a technician assigned to a scheduled appointment.");
                t.HasCheckConstraint("CK_FgsAppointmentAssignment_EstimatedHours", "\"EstimatedHours\" > 0");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsAppointmentAssignment");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.AppointmentId).HasComment("Appointment associated with the assignment.");
        entity.Property(e => e.EmployeeId).HasComment("Employee assigned to the appointment. References user service; no FK by design.");
        entity.Property(e => e.CrewId).HasComment("Crew assignment snapshot at the time of scheduling. References setup service; no FK by design.");
        entity.Property(e => e.ServiceDate).HasColumnType("date").IsRequired()
            .HasComment("Scheduled service date for the technician assignment.");
        entity.Property(e => e.ScheduledTime).HasColumnType("time").IsRequired()
            .HasComment("Scheduled local start time for the technician assignment.");
        entity.Property(e => e.EstimatedHours).HasColumnType("numeric(8,2)").IsRequired()
            .HasComment("Estimated hours assigned to the technician.");
        entity.Property(e => e.ActualStartOn).HasColumnType("timestamptz")
            .HasComment("System-maintained start timestamp derived from assignment events.");
        entity.Property(e => e.ActualEndOn).HasColumnType("timestamptz")
            .HasComment("System-maintained end timestamp derived from assignment events.");
        entity.Property(e => e.AssignedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the technician was assigned.");
        entity.Property(e => e.AssignedBy).IsRequired().HasComment("User who assigned the technician.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).IsRequired().HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<FgsAppointment>()
            .WithMany()
            .HasForeignKey(e => e.AppointmentId)
            .HasConstraintName("FK_FgsAppointmentAssignment_Appointment")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AppointmentId, e.EmployeeId })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAppointmentAssignment_AppointmentEmployee");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AppointmentId }).HasDatabaseName("IX_FgsAppointmentAssignment_Appointment");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId }).HasDatabaseName("IX_FgsAppointmentAssignment_Employee");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceDate }).HasDatabaseName("IX_FgsAppointmentAssignment_ServiceDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId, e.ServiceDate, e.ScheduledTime })
            .HasDatabaseName("IX_FgsAppointmentAssignment_EmployeeSchedule");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CrewId, e.ServiceDate }).HasDatabaseName("IX_FgsAppointmentAssignment_Crew");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId, e.ActualStartOn, e.ActualEndOn })
            .HasDatabaseName("IX_FgsAppointmentAssignment_Overlap");
    }
}
