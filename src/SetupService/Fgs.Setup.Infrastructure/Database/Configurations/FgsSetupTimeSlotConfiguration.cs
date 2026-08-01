using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupTimeSlotConfiguration : IEntityTypeConfiguration<FgsSetupTimeSlot>
{
    public void Configure(EntityTypeBuilder<FgsSetupTimeSlot> entity)
    {
        entity.ToTable("FgsSetupTimeSlot");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupTimeSlot");
        entity.HasOne<FgsSetupZone>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupZoneId)
            .HasConstraintName("FK_FgsSetupTimeSlot_Zone")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => e.FgsSetupZoneId)
            .HasDatabaseName("IX_FgsSetupTimeSlot_ZoneId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupZoneId })
            .HasDatabaseName("IX_FgsSetupTimeSlot_Zone");
        entity.Property(e => e.IncludeInCapacityPlanning)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment(
                "Indicates whether this time slot is considered during capacity planning and scheduling calculations. When false, the time slot is excluded from capacity planning.");
        entity.Property(e => e.ShowToExternalSystem)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment(
                "Indicates whether this time slot is exposed to third-party integrations and external systems. When false, the time slot remains internal to the application.");
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsSetupTimeSlot_Code_Upper",
                "\"Code\" = UPPER(\"Code\")");
            t.HasCheckConstraint(
                "CK_FgsSetupTimeSlot_TimeRange",
                "\"EndTime\" > \"BeginTime\"");
        });
    }
}
