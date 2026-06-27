using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class FgsEmployeeTechnicianProfileConfiguration : IEntityTypeConfiguration<FgsEmployeeTechnicianProfile>
{
    public void Configure(EntityTypeBuilder<FgsEmployeeTechnicianProfile> entity)
    {
        entity.ToTable(
            "FgsEmployeeTechnicianProfile",
            t => t.HasComment(
                "Stores technician-specific operational settings used by dispatching, scheduling, routing, capacity planning, inventory assignment, and customer-facing technician communications."));

        entity.HasKey(e => e.Id).HasName("PK_FgsEmployeeTechnicianProfile");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);

        entity.Property(e => e.Id).HasComment("Primary key identifier for the technician profile.");
        entity.Property(e => e.TenantId).HasComment("Identifier of the tenant that owns the technician profile.");
        entity.Property(e => e.CompanyId).HasComment("Identifier of the company that owns the technician profile.");
        entity.Property(e => e.EmployeeId)
            .HasComment("Reference to the employee associated with this technician profile. One employee may have only one technician profile.");
        entity.Property(e => e.TechCode).HasMaxLength(25).IsRequired()
            .HasComment("Required unique technician code used on dispatch boards, whiteboards, reports, scheduling screens, mobile applications, and integrations.");
        entity.Property(e => e.TechName).HasMaxLength(100)
            .HasComment("Technician name displayed to customers in appointment reminders, technician tracking pages, work orders, invoices, and customer communications.");
        entity.Property(e => e.CanBeScheduled).HasDefaultValue(true)
            .HasComment("Indicates whether the technician can receive appointments and appear on the dispatch board.");
        entity.Property(e => e.DailyCapacityHours).HasColumnType("numeric(5,2)").HasDefaultValue(8.00m)
            .HasComment("Number of labor hours available per day. Used for whiteboard capacity calculations, scheduling, utilization reporting, and workforce planning.");
        entity.Property(e => e.DispatchZoneId)
            .HasComment("Default dispatch zone assigned to the technician for territory-based scheduling and routing.");
        entity.Property(e => e.StartLocationTypeId)
            .HasComment("Indicates where the technician normally starts the workday. Typical values are Office or Home.");
        entity.Property(e => e.StartTime).HasColumnType("time")
            .HasComment("Default daily start time used for scheduling, route planning, technician availability calculations, and capacity management.");
        entity.Property(e => e.TechTradeId)
            .HasComment("Primary trade classification assigned to the technician such as HVAC, Plumbing, Electrical, Refrigeration, Landscaping, Cleaning, or Pest Control.");
        entity.Property(e => e.TechSkillId)
            .HasComment("Primary skill or specialization assigned to the technician within the selected trade.");
        entity.Property(e => e.TruckId)
            .HasComment("Assigned service vehicle used by the technician. Used for dispatching, route planning, truck inventory, inventory consumption, and replenishment processes.");
        entity.Property(e => e.CustomerFacingPhone).HasMaxLength(25)
            .HasComment("Phone number displayed to customers for technician communication. May differ from the employee personal or office phone number.");
        entity.Property(e => e.Notes).HasColumnType("text")
            .HasComment("Internal technician-specific notes used by dispatchers, supervisors, and managers.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()")
            .HasComment("Date and time the technician profile was created.");
        entity.Property(e => e.CreatedBy).IsRequired()
            .HasComment("User who created the technician profile.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp")
            .HasComment("Date and time the technician profile was last modified.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User who last modified the technician profile.");

        entity.HasOne<FgsEmployee>()
            .WithMany()
            .HasForeignKey(e => e.EmployeeId)
            .HasConstraintName("FK_FgsEmployeeTechnicianProfile_FgsEmployee_EmployeeId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEmployeeTechnicianProfile_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CanBeScheduled })
            .HasDatabaseName("IX_FgsEmployeeTechnicianProfile_TenantId_CompanyId_CanBeScheduled");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DispatchZoneId })
            .HasDatabaseName("IX_FgsEmployeeTechnicianProfile_TenantId_CompanyId_DispatchZoneId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TechTradeId })
            .HasDatabaseName("IX_FgsEmployeeTechnicianProfile_TenantId_CompanyId_TechTradeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId })
            .IsUnique()
            .HasDatabaseName("UX_FgsEmployeeTechnicianProfile_EmployeeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TechCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsEmployeeTechnicianProfile_TenantId_CompanyId_TechCode");
    }
}
