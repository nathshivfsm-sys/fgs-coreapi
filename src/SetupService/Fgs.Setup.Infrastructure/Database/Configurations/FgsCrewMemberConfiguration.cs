using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class FgsCrewMemberConfiguration : IEntityTypeConfiguration<FgsCrewMember>
{
    public void Configure(EntityTypeBuilder<FgsCrewMember> entity)
    {
        entity.ToTable(
            "FgsCrewMember",
            t => t.HasComment("Stores permanent technician membership within crews."));

        entity.HasKey(e => e.Id).HasName("PK_FgsCrewMember");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.CrewId).HasComment("Crew to which the technician belongs.");
        entity.Property(e => e.TechnicianProfileId).HasComment("Technician profile assigned to the crew.");
        entity.Property(e => e.IsLead).HasDefaultValue(false)
            .HasComment("Indicates whether the technician is the designated lead technician for the crew.");
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);

        entity.HasOne<FgsCrew>()
            .WithMany()
            .HasForeignKey(e => e.CrewId)
            .HasConstraintName("FK_FgsCrewMember_FgsCrew_CrewId")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<FgsEmployeeTechnicianProfile>()
            .WithMany()
            .HasForeignKey(e => e.TechnicianProfileId)
            .HasConstraintName("FK_FgsCrewMember_FgsEmployeeTechnicianProfile_TechnicianProfileId")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsCrewMember_TenantId_CompanyId");
        entity.HasIndex(e => e.CrewId)
            .HasDatabaseName("IX_FgsCrewMember_CrewId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TechnicianProfileId })
            .IsUnique()
            .HasDatabaseName("UX_FgsCrewMember_TenantId_CompanyId_TechnicianProfileId");
        entity.HasIndex(e => new { e.CrewId, e.IsLead })
            .IsUnique()
            .HasFilter("\"IsLead\" = true")
            .HasDatabaseName("UX_FgsCrewMember_CrewId_Lead");
    }
}
