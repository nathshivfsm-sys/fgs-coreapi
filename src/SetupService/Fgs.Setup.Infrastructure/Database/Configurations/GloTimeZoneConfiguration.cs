using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloTimeZoneConfiguration : IEntityTypeConfiguration<GloTimeZone>
{
    public void Configure(EntityTypeBuilder<GloTimeZone> entity)
    {
        entity.ToTable(
            "GloTimeZone",
            t => t.HasComment(
                "Global reference table containing time zone options available for customer and user selection."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the time zone.");

        entity.Property(e => e.TimeZoneCode)
            .HasMaxLength(10)
            .HasComment(
                "Short time zone abbreviation used by the application, such as EST, CST, MST, or PST.");
        entity.Property(e => e.Name)
            .HasMaxLength(50)
            .HasComment("User-friendly display name for the time zone.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the time zone is currently available for selection.");

        entity.HasIndex(e => e.TimeZoneCode)
            .IsUnique()
            .HasDatabaseName("IX_GloTimeZone_TimeZoneCode");
    }
}
