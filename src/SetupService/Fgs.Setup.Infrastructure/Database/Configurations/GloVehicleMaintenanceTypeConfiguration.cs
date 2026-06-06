using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloVehicleMaintenanceTypeConfiguration : IEntityTypeConfiguration<GloVehicleMaintenanceType>
{
    public void Configure(EntityTypeBuilder<GloVehicleMaintenanceType> entity)
    {
        entity.ToTable(
            "GloVehicleMaintenanceType",
            t => t.HasComment(
                "Stores standard vehicle maintenance types used when recording maintenance activities for company vehicles."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key.");

        entity.Property(e => e.MaintenanceTypeCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Unique system code identifying the maintenance type.");

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("Display name of the maintenance type.");

        entity.Property(e => e.Description)
            .HasMaxLength(500)
            .HasComment("Description of the maintenance type.");

        entity.Property(e => e.DisplayOrder)
            .HasColumnType("smallint")
            .HasDefaultValue((short)1)
            .HasComment("Controls display order in lists and dropdowns.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the maintenance type is active and available for selection.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");

        entity.HasAlternateKey(e => e.MaintenanceTypeCode)
            .HasName("UQ_GloVehicleMaintenanceType_MaintenanceTypeCode");

        entity.HasIndex(e => e.DisplayOrder)
            .HasDatabaseName("IX_GloVehicleMaintenanceType_DisplayOrder");
    }
}
