using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloTimeSlotConfiguration : IEntityTypeConfiguration<GloTimeSlot>
{
    public void Configure(EntityTypeBuilder<GloTimeSlot> entity)
    {
        entity.ToTable("GloTimeSlot");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.Code)
            .IsUnique()
            .HasDatabaseName("UQ_GloTimeSlot_Code");
        entity.Property(e => e.Code).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.BeginTime).HasColumnType("interval");
        entity.Property(e => e.EndTime).HasColumnType("interval");
        entity.Property(e => e.MarkTechArrivedLateAfter).HasColumnType("interval");
        entity.Property(e => e.MarkWorkOrderDelayedCompletionAfter).HasColumnType("interval");
        entity.Property(e => e.IsMobileVisible).HasDefaultValue(true);
        entity.Property(e => e.IsCustomerPortalVisible).HasDefaultValue(true);
        entity.Property(e => e.IncludeInCapacityPlanning).HasDefaultValue(false);
        entity.Property(e => e.ShowToExternalSystem).HasDefaultValue(false);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.ToTable(t =>
        {
            t.HasCheckConstraint("CK_GloTimeSlot_Code_Upper", "\"Code\" = UPPER(\"Code\")");
            t.HasCheckConstraint("CK_GloTimeSlot_TimeRange", "\"EndTime\" > \"BeginTime\"");
        });
    }
}
