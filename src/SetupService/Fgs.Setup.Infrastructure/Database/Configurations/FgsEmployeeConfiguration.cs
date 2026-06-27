using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class FgsEmployeeConfiguration : IEntityTypeConfiguration<FgsEmployee>
{
    public void Configure(EntityTypeBuilder<FgsEmployee> entity)
    {
        entity.ToTable(
            "FgsEmployee",
            t => t.HasComment(
                "Stores employee master information for office and field personnel. Employees may optionally be linked to a system user account through UserId. Technician-specific operational settings are stored in FgsEmployeeTechnicianProfile."));

        entity.HasKey(e => e.Id).HasName("PK_FgsEmployee");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);

        entity.Property(e => e.Id).HasComment("Primary key identifier for the employee record.");
        entity.Property(e => e.TenantId).HasComment("Identifier of the tenant that owns the employee record.");
        entity.Property(e => e.CompanyId).HasComment("Identifier of the company that owns the employee record.");
        entity.Property(e => e.UserId)
            .HasComment("Optional reference to the system user account associated with this employee. One user may be linked to only one employee. References identity service; no FK by design.");
        entity.Property(e => e.EmployeeNumber)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Unique employee number within the company.");
        entity.Property(e => e.EmployeeTypeId)
            .HasComment("Employee classification. Typical values are Office and Technician.");
        entity.Property(e => e.DisplayName)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Display name used throughout the application, dispatch board, schedules, and reports.");
        entity.Property(e => e.LegalFirstName).HasMaxLength(100).IsRequired()
            .HasComment("Employee legal first name.");
        entity.Property(e => e.LegalMiddleName).HasMaxLength(100)
            .HasComment("Employee legal middle name.");
        entity.Property(e => e.LegalLastName).HasMaxLength(100).IsRequired()
            .HasComment("Employee legal last name.");
        entity.Property(e => e.BirthDate).HasComment("Employee date of birth.");
        entity.Property(e => e.HireDate).HasComment("Date employee was hired.");
        entity.Property(e => e.TerminationDate).HasComment("Date employee was terminated or separated from employment.");
        entity.Property(e => e.StatusId)
            .HasComment("Current employee status such as Active, Inactive, Leave of Absence, or Terminated.");
        entity.Property(e => e.PersonalEmail).HasMaxLength(255)
            .HasComment("Employee personal email address.");
        entity.Property(e => e.OfficeEmail).HasMaxLength(255)
            .HasComment("Employee company or office email address.");
        entity.Property(e => e.PersonalPhone).HasMaxLength(25)
            .HasComment("Employee personal phone number.");
        entity.Property(e => e.OfficePhone).HasMaxLength(25)
            .HasComment("Employee office phone number or extension.");
        entity.Property(e => e.AddressId)
            .HasComment("Reference to the employee mailing or home address record. No FK by design.");
        entity.Property(e => e.ProfilePhotoFileId)
            .HasComment("Identifier of the employee profile photo stored in the file repository. No FK by design.");
        entity.Property(e => e.RegularRate).HasColumnType("numeric(18,2)")
            .HasComment("Standard hourly labor rate used for payroll, costing, and reporting.");
        entity.Property(e => e.OvertimeRate).HasColumnType("numeric(18,2)")
            .HasComment("Overtime hourly labor rate.");
        entity.Property(e => e.DoubleTimeRate).HasColumnType("numeric(18,2)")
            .HasComment("Double-time hourly labor rate.");
        entity.Property(e => e.LaborBurdenTypeId)
            .HasComment("Determines whether labor burden is expressed as a percentage or fixed amount.");
        entity.Property(e => e.LaborBurdenValue).HasColumnType("numeric(18,2)")
            .HasComment("Labor burden amount or percentage used for estimating, costing, and profitability calculations.");
        entity.Property(e => e.IsPurchaser).HasDefaultValue(false)
            .HasComment("Indicates whether the employee is authorized to create or approve purchase orders.");
        entity.Property(e => e.Notes).HasColumnType("text")
            .HasComment("Internal notes related to the employee.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()")
            .HasComment("Date and time the employee record was created.");
        entity.Property(e => e.CreatedBy).IsRequired()
            .HasComment("User who created the employee record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp")
            .HasComment("Date and time the employee record was last modified.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User who last modified the employee record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEmployee_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeTypeId })
            .HasDatabaseName("IX_FgsEmployee_TenantId_CompanyId_EmployeeTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.StatusId })
            .HasDatabaseName("IX_FgsEmployee_TenantId_CompanyId_StatusId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayName })
            .HasDatabaseName("IX_FgsEmployee_TenantId_CompanyId_DisplayName");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeNumber })
            .IsUnique()
            .HasDatabaseName("UX_FgsEmployee_TenantId_CompanyId_EmployeeNumber");
        entity.HasIndex(e => e.UserId)
            .IsUnique()
            .HasFilter("\"UserId\" IS NOT NULL")
            .HasDatabaseName("UX_FgsEmployee_UserId");
    }
}
