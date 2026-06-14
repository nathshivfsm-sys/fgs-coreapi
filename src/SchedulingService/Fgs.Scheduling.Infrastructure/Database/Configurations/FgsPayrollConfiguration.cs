using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsPayrollConfiguration : IEntityTypeConfiguration<FgsPayroll>
{
    public void Configure(EntityTypeBuilder<FgsPayroll> entity)
    {
        entity.ToTable(
            "FgsPayroll",
            t =>
            {
                t.HasComment("Stores payroll results for a single employee within a payroll pay period.");
                t.HasCheckConstraint("CK_FgsPayroll_BurdenType", "\"BurdenTypeId\" IN ('P', 'F')");
                t.HasCheckConstraint(
                    "CK_FgsPayroll_Signature",
                    "(\"SignedOn\" IS NULL AND \"SignatureFileId\" IS NULL AND \"SignedBy\" IS NULL) OR (\"SignedOn\" IS NOT NULL AND \"SignatureFileId\" IS NOT NULL AND \"SignedBy\" IS NOT NULL)");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsPayroll");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.PayPeriodId).HasComment("Payroll pay period associated with this payroll record.");
        entity.Property(e => e.EmployeeId).HasComment("Employee associated with this payroll record. References user service; no FK by design.");
        entity.Property(e => e.EmployeeNumber).HasMaxLength(50)
            .HasComment("Employee number snapshot captured at payroll calculation time.");
        entity.Property(e => e.EmployeeName).HasMaxLength(200).IsRequired()
            .HasComment("Employee name snapshot captured at payroll calculation time.");
        entity.Property(e => e.RegularHours).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Regular hours included in payroll calculation.");
        entity.Property(e => e.OvertimeHours).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Overtime hours included in payroll calculation.");
        entity.Property(e => e.DoubleTimeHours).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Double-time hours included in payroll calculation.");
        entity.Property(e => e.RegularRate).HasColumnType("numeric(18,4)").HasDefaultValue(0m).IsRequired()
            .HasComment("Regular pay rate snapshot at calculation time.");
        entity.Property(e => e.OvertimeRate).HasColumnType("numeric(18,4)").HasDefaultValue(0m).IsRequired()
            .HasComment("Overtime pay rate snapshot at calculation time.");
        entity.Property(e => e.DoubleTimeRate).HasColumnType("numeric(18,4)").HasDefaultValue(0m).IsRequired()
            .HasComment("Double-time pay rate snapshot at calculation time.");
        entity.Property(e => e.RegularAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Regular pay amount.");
        entity.Property(e => e.OvertimeAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Overtime pay amount.");
        entity.Property(e => e.DoubleTimeAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Double-time pay amount.");
        entity.Property(e => e.CommissionAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Commission amount included in payroll.");
        entity.Property(e => e.BonusAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Bonus amount included in payroll.");
        entity.Property(e => e.AdjustmentAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Positive or negative payroll adjustment amount.");
        entity.Property(e => e.BurdenTypeId).HasMaxLength(1).HasDefaultValue("P").IsRequired()
            .HasComment("Burden calculation method. P=Percent, F=Fixed Amount.");
        entity.Property(e => e.BurdenValue).HasColumnType("numeric(18,4)").HasDefaultValue(0m).IsRequired()
            .HasComment("Burden percentage or fixed amount snapshot used during payroll calculation.");
        entity.Property(e => e.BurdenAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Calculated burden amount used for costing and profitability reporting.");
        entity.Property(e => e.GrossPayAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Total gross pay exported to the payroll provider.");
        entity.Property(e => e.SignatureFileId).HasComment("Reference to employee payroll acknowledgement signature document.");
        entity.Property(e => e.SignedOn).HasColumnType("timestamptz")
            .HasComment("Date and time payroll acknowledgement was signed.");
        entity.Property(e => e.SignedBy).HasMaxLength(200)
            .HasComment("Name of person who signed the payroll acknowledgement.");
        entity.Property(e => e.Notes).HasColumnType("text")
            .HasComment("Optional payroll notes, explanations and adjustment reasons.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).IsRequired().HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<FgsPayrollPayPeriod>()
            .WithMany()
            .HasForeignKey(e => e.PayPeriodId)
            .HasConstraintName("FK_FgsPayroll_FgsPayrollPayPeriod")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsPayroll_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PayPeriodId }).HasDatabaseName("IX_FgsPayroll_PayPeriod");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId }).HasDatabaseName("IX_FgsPayroll_Employee");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SignedOn }).HasDatabaseName("IX_FgsPayroll_SignedOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PayPeriodId, e.EmployeeId })
            .IsUnique()
            .HasDatabaseName("UX_FgsPayroll_PayPeriodEmployee");
    }
}
