using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsSetupTimeSlotConfiguration : IEntityTypeConfiguration<FgsSetupTimeSlot>
{
    public void Configure(EntityTypeBuilder<FgsSetupTimeSlot> entity)
    {
        entity.ToTable("FgsSetupTimeSlot");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.ConfigureTenantCompanySetupFk("FK_FgsSetupTimeSlot_FgsTenantCompany_TenantId_CompanyId");
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
