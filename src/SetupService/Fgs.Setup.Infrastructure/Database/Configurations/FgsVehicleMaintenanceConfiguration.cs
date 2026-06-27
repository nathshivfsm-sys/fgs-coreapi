using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsVehicleMaintenanceConfiguration : IEntityTypeConfiguration<FgsVehicleMaintenance>
{
    public void Configure(EntityTypeBuilder<FgsVehicleMaintenance> entity)
    {
        entity.ToTable(
            "FgsVehicleMaintenance",
            t => t.HasComment(
                "Stores completed and scheduled maintenance activities, inspections, repairs, and service history for company vehicles."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key.");

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");

        entity.Property(e => e.VehicleId)
            .HasComment("Vehicle that received or is scheduled to receive maintenance service.");

        entity.Property(e => e.VehicleMaintenanceTypeId)
            .HasComment("Type of maintenance activity being performed or scheduled.");

        entity.Property(e => e.ServiceDate)
            .HasComment("Date the maintenance was performed or is scheduled to be performed.");

        entity.Property(e => e.MileageAtService)
            .HasComment("Vehicle odometer reading at the time the maintenance was performed.");

        entity.Property(e => e.ServiceProvider)
            .HasMaxLength(200)
            .HasComment("Name of the repair shop, dealership, service provider, or maintenance vendor.");

        entity.Property(e => e.InvoiceNumber)
            .HasMaxLength(100)
            .HasComment(
                "Vendor invoice, receipt, repair order, or work order number associated with the maintenance activity.");

        entity.Property(e => e.Cost)
            .HasColumnType("numeric(18,2)")
            .HasComment("Total cost incurred for the maintenance activity.");

        entity.Property(e => e.NextServiceDate)
            .HasComment("Recommended next service date based on maintenance provider recommendations.");

        entity.Property(e => e.NextServiceMileage)
            .HasComment("Recommended next service mileage based on maintenance provider recommendations.");

        entity.Property(e => e.IsCompleted)
            .HasDefaultValue(true)
            .HasComment(
                "Indicates whether the maintenance activity has been completed. False indicates a scheduled or pending maintenance item.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the maintenance record is active and available for use.");

        entity.Property(e => e.Description)
            .HasMaxLength(500)
            .HasComment("Short summary of the maintenance activity performed or scheduled.");

        entity.Property(e => e.Notes)
            .HasColumnType("text")
            .HasComment(
                "Detailed notes, observations, recommendations, or repair information related to the maintenance activity.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User who created the record.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.VehicleId })
            .HasDatabaseName("IX_FgsVehicleMaintenance_TenantId_CompanyId_VehicleId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceDate })
            .HasDatabaseName("IX_FgsVehicleMaintenance_TenantId_CompanyId_ServiceDate");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.VehicleMaintenanceTypeId })
            .HasDatabaseName("IX_FgsVehicleMaintenance_TenantId_CompanyId_VehicleMaintenanceTypeId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.NextServiceDate })
            .HasDatabaseName("IX_FgsVehicleMaintenance_TenantId_CompanyId_NextServiceDate");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsCompleted })
            .HasDatabaseName("IX_FgsVehicleMaintenance_TenantId_CompanyId_IsCompleted");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsVehicleMaintenance_TenantId_CompanyId_IsActive");

        entity.HasOne<FgsVehicle>()
            .WithMany()
            .HasForeignKey(e => e.VehicleId)
            .HasConstraintName("FK_FgsVehicleMaintenance_FgsVehicle_VehicleId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
