using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsCrewMemberConfiguration : IEntityTypeConfiguration<FgsCrewMember>
{
    public void Configure(EntityTypeBuilder<FgsCrewMember> entity)
    {
        entity.ToTable(
            "FgsCrewMember",
            t => t.HasComment("Stores technician membership within a crew."));

        entity.HasKey(e => e.Id).HasName("PK_FgsCrewMember");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.CrewId).HasComment("Crew associated with the technician.");
        entity.Property(e => e.EmployeeId).HasComment("Employee assigned to the crew. References user service; no FK by design.");
        entity.Property(e => e.IsLead).HasDefaultValue(false).IsRequired()
            .HasComment("Indicates whether the employee is the lead technician or foreman for the crew.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).IsRequired().HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<FgsCrew>()
            .WithMany()
            .HasForeignKey(e => e.CrewId)
            .HasConstraintName("FK_FgsCrewMember_FgsCrew")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsCrewMember_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CrewId }).HasDatabaseName("IX_FgsCrewMember_Crew");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId }).HasDatabaseName("IX_FgsCrewMember_Employee");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CrewId, e.IsLead }).HasDatabaseName("IX_FgsCrewMember_IsLead");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EmployeeId })
            .IsUnique()
            .HasDatabaseName("UX_FgsCrewMember_Employee");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CrewId })
            .IsUnique()
            .HasFilter("\"IsLead\" = true")
            .HasDatabaseName("UX_FgsCrewMember_LeadPerCrew");
    }
}
