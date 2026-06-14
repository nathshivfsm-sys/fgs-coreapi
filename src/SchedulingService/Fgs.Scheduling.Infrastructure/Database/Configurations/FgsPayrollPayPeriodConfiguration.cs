using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsPayrollPayPeriodConfiguration : IEntityTypeConfiguration<FgsPayrollPayPeriod>
{
    public void Configure(EntityTypeBuilder<FgsPayrollPayPeriod> entity)
    {
        entity.ToTable(
            "FgsPayrollPayPeriod",
            t =>
            {
                t.HasComment("Defines payroll processing periods used to calculate, approve and export payroll.");
                t.HasCheckConstraint("CK_FgsPayrollPayPeriod_DateRange", "\"PeriodEndDate\" >= \"PeriodStartDate\"");
                t.HasCheckConstraint("CK_FgsPayrollPayPeriod_Status", "\"PayrollStatusId\" IN (1, 2, 3, 4)");
                t.HasCheckConstraint(
                    "CK_FgsPayrollPayPeriod_Calculated",
                    "(\"CalculatedOn\" IS NULL AND \"CalculatedBy\" IS NULL) OR (\"CalculatedOn\" IS NOT NULL AND \"CalculatedBy\" IS NOT NULL)");
                t.HasCheckConstraint(
                    "CK_FgsPayrollPayPeriod_Approved",
                    "(\"ApprovedOn\" IS NULL AND \"ApprovedBy\" IS NULL) OR (\"ApprovedOn\" IS NOT NULL AND \"ApprovedBy\" IS NOT NULL)");
                t.HasCheckConstraint(
                    "CK_FgsPayrollPayPeriod_Exported",
                    "(\"ExportedOn\" IS NULL AND \"ExportedBy\" IS NULL) OR (\"ExportedOn\" IS NOT NULL AND \"ExportedBy\" IS NOT NULL)");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsPayrollPayPeriod");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.PayPeriodCode).HasMaxLength(20).IsRequired()
            .HasComment("Human-readable payroll period code such as 2026-PP12, 2026-06A or 2026-06B.");
        entity.Property(e => e.PeriodStartDate).HasColumnType("date").IsRequired()
            .HasComment("Inclusive payroll period start date.");
        entity.Property(e => e.PeriodEndDate).HasColumnType("date").IsRequired()
            .HasComment("Inclusive payroll period end date.");
        entity.Property(e => e.PayrollStatusId).HasDefaultValue((short)1).IsRequired()
            .HasComment("Payroll status. 1=Open, 2=Calculated, 3=Approved, 4=Exported.");
        entity.Property(e => e.CalculatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time payroll calculations were generated.");
        entity.Property(e => e.CalculatedBy).HasComment("User who generated payroll calculations.");
        entity.Property(e => e.ApprovedOn).HasColumnType("timestamptz")
            .HasComment("Date and time payroll was approved.");
        entity.Property(e => e.ApprovedBy).HasComment("User who approved payroll.");
        entity.Property(e => e.ExportedOn).HasColumnType("timestamptz")
            .HasComment("Date and time payroll was exported.");
        entity.Property(e => e.ExportedBy).HasComment("User who exported payroll.");
        entity.Property(e => e.ExportReference).HasMaxLength(100)
            .HasComment("Optional external payroll batch number, export file identifier or payroll provider reference.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).IsRequired().HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsPayrollPayPeriod_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PayrollStatusId }).HasDatabaseName("IX_FgsPayrollPayPeriod_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PeriodStartDate }).HasDatabaseName("IX_FgsPayrollPayPeriod_StartDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PeriodEndDate }).HasDatabaseName("IX_FgsPayrollPayPeriod_EndDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PayPeriodCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsPayrollPayPeriod_PayPeriodCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PeriodStartDate, e.PeriodEndDate })
            .IsUnique()
            .HasDatabaseName("UX_FgsPayrollPayPeriod_DateRange");
    }
}
