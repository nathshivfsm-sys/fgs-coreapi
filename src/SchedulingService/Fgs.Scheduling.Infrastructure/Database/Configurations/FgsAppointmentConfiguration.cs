using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsAppointmentConfiguration : IEntityTypeConfiguration<FgsAppointment>
{
    public void Configure(EntityTypeBuilder<FgsAppointment> entity)
    {
        entity.ToTable(
            "FgsAppointment",
            t =>
            {
                t.HasComment("Represents a scheduled customer visit for a lead, opportunity or work order.");
                t.HasCheckConstraint("CK_FgsAppointment_EstimatedHours", "\"EstimatedHours\" > 0");
                t.HasCheckConstraint("CK_FgsAppointment_Status", "\"AppointmentStatusId\" IN (1, 2, 3)");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsAppointment");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.SourceTypeId).HasComment("Source type. Typically Lead, Opportunity or Work Order.");
        entity.Property(e => e.SourceId).HasComment("Identifier of the source record.");
        entity.Property(e => e.CrewId).HasComment("Scheduled crew assigned to the appointment.");
        entity.Property(e => e.CustomerContactName).HasMaxLength(200)
            .HasComment("Contact name used for appointment reminders and confirmations.");
        entity.Property(e => e.ServiceDate).HasColumnType("date").IsRequired()
            .HasComment("Customer promised service date.");
        entity.Property(e => e.ScheduledTime).HasColumnType("time").IsRequired()
            .HasComment("Customer promised local appointment time.");
        entity.Property(e => e.EstimatedHours).HasColumnType("numeric(8,2)").IsRequired()
            .HasComment("Estimated appointment duration used for scheduling and dispatch planning.");
        entity.Property(e => e.AppointmentStatusId).HasComment("Appointment status. 1=Unassigned, 2=Open, 3=Completed.");
        entity.Property(e => e.CustomerApprovedOn).HasColumnType("timestamptz")
            .HasComment("Date and time customer approved the appointment visit.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).IsRequired().HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<FgsCrew>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId, e.CrewId })
            .HasPrincipalKey(c => new { c.TenantId, c.CompanyId, c.Id })
            .HasConstraintName("FK_FgsAppointment_Crew")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SourceTypeId, e.SourceId }).HasDatabaseName("IX_FgsAppointment_Source");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceDate }).HasDatabaseName("IX_FgsAppointment_ServiceDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AppointmentStatusId }).HasDatabaseName("IX_FgsAppointment_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CrewId }).HasDatabaseName("IX_FgsAppointment_Crew");
    }
}
