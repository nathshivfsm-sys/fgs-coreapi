using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsPayrollLineConfiguration : IEntityTypeConfiguration<FgsPayrollLine>
{
    public void Configure(EntityTypeBuilder<FgsPayrollLine> entity)
    {
        entity.ToTable(
            "FgsPayrollLine",
            t =>
            {
                t.HasComment("Stores payroll detail lines associated with a payroll record.");
                t.HasCheckConstraint("CK_FgsPayrollLine_Type", "\"PayrollLineTypeId\" IN (1, 2, 3)");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsPayrollLine");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.PayrollId).HasComment("Parent payroll record.");
        entity.Property(e => e.PayrollLineTypeId).HasComment("Payroll line type. 1=Commission, 2=Bonus, 3=Adjustment.");
        entity.Property(e => e.Description).HasMaxLength(250).IsRequired()
            .HasComment("User-facing payroll line description.");
        entity.Property(e => e.Amount).HasColumnType("numeric(18,2)").HasDefaultValue(0m).IsRequired()
            .HasComment("Positive or negative payroll line amount.");
        entity.Property(e => e.Notes).HasColumnType("text").HasComment("Optional notes and explanation for the payroll line.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).IsRequired().HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<FgsPayroll>()
            .WithMany()
            .HasForeignKey(e => e.PayrollId)
            .HasConstraintName("FK_FgsPayrollLine_FgsPayroll")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsPayrollLine_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PayrollId }).HasDatabaseName("IX_FgsPayrollLine_Payroll");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PayrollLineTypeId }).HasDatabaseName("IX_FgsPayrollLine_Type");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PayrollId, e.PayrollLineTypeId })
            .HasDatabaseName("IX_FgsPayrollLine_PayrollType");
    }
}
