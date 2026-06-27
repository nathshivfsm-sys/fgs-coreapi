using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsDispatchBoardTechnicianConfiguration : IEntityTypeConfiguration<FgsDispatchBoardTechnician>
{
    public void Configure(EntityTypeBuilder<FgsDispatchBoardTechnician> entity)
    {
        entity.ToTable(
            "FgsDispatchBoardTechnician",
            t =>
            {
                t.HasComment("Stores daily dispatch board technician projections used for scheduling and dispatching.");
                t.HasCheckConstraint(
                    "CK_FgsDispatchBoardTechnician_Status",
                    "\"DispatchBoardStatusId\" IN (0, 1, 2, 3, 4, 5, 6)");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsDispatchBoardTechnician");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.ServiceDate).HasColumnType("date").IsRequired()
            .HasComment("Service date represented on the dispatch board.");
        entity.Property(e => e.TechnicianProfileId)
            .HasComment("Reference to setup.FgsEmployeeTechnicianProfile. Stored without cross-domain foreign key.");
        entity.Property(e => e.TechCode).HasMaxLength(25).IsRequired()
            .HasComment("Technician code snapshot used by dispatch board displays.");
        entity.Property(e => e.TechName).HasMaxLength(200).IsRequired()
            .HasComment("Technician name snapshot used by dispatch board displays.");
        entity.Property(e => e.CrewId)
            .HasComment("Daily crew assignment identifier. May be overridden for a specific service date.");
        entity.Property(e => e.CrewCode).HasMaxLength(25)
            .HasComment("Crew code snapshot used for dispatch board grouping.");
        entity.Property(e => e.CrewName).HasMaxLength(100)
            .HasComment("Crew name snapshot used for dispatch board grouping.");
        entity.Property(e => e.DispatchBoardStatusId).HasDefaultValue((short)0)
            .HasComment("Dispatch board status. 0=Available, 1=Assigned, 2=Dispatched, 3=Arrived, 4=Waiting, 5=Completed, 6=Off Duty.");
        entity.Property(e => e.IsWorking).HasDefaultValue(true)
            .HasComment("Indicates whether the technician should appear on the dispatch board for the service date.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100)
            .HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100)
            .HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsDispatchBoardTechnician_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceDate })
            .HasDatabaseName("IX_FgsDispatchBoardTechnician_ServiceDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceDate, e.CrewId })
            .HasDatabaseName("IX_FgsDispatchBoardTechnician_Crew");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceDate, e.TechnicianProfileId })
            .IsUnique()
            .HasDatabaseName("UX_FgsDispatchBoardTechnician_ServiceDate_TechnicianProfileId");
    }
}
