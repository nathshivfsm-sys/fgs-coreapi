using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsVehicleConfiguration : IEntityTypeConfiguration<FgsVehicle>
{
    public void Configure(EntityTypeBuilder<FgsVehicle> entity)
    {
        entity.ToTable(
            "FgsVehicle",
            t =>
            {
                t.HasComment(
                    "Stores company-owned or leased vehicles used for field service operations. Each vehicle is associated with a truck warehouse that serves as an inventory location.");
                t.HasCheckConstraint(
                    "CK_FgsVehicle_OwnershipType",
                    "\"OwnershipType\" IN ('Owned', 'Leased', 'Rented')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");

        entity.Property(e => e.WarehouseId)
            .HasComment("Associated truck warehouse used as the vehicle inventory location.");

        entity.Property(e => e.OwnershipType)
            .HasMaxLength(20)
            .IsRequired()
            .HasDefaultValue(VehicleOwnershipTypes.Owned)
            .HasComment("Indicates whether the vehicle is owned, leased, or rented.");

        entity.Property(e => e.OwnershipCompany)
            .HasMaxLength(200)
            .HasComment(
                "Name of the leasing company, rental provider, or other organization that owns the vehicle when it is not company-owned.");

        entity.Property(e => e.Year).HasComment("Vehicle model year.");
        entity.Property(e => e.Make)
            .HasMaxLength(100)
            .HasComment("Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc.");

        entity.Property(e => e.Model)
            .HasMaxLength(100)
            .HasComment("Vehicle model such as F-150, Transit, Silverado, Express, etc.");

        entity.Property(e => e.Color)
            .HasMaxLength(50)
            .HasComment("Vehicle exterior color.");

        entity.Property(e => e.VIN)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Vehicle Identification Number assigned by the manufacturer.");

        entity.Property(e => e.LicensePlate)
            .HasMaxLength(50)
            .HasComment("Vehicle registration plate number.");

        entity.Property(e => e.LicensePlateState)
            .HasMaxLength(50)
            .HasComment("State or province issuing the vehicle registration.");

        entity.Property(e => e.PurchaseDate).HasComment("Date the vehicle was purchased or acquired.");

        entity.Property(e => e.PurchasePrice)
            .HasColumnType("numeric(18,2)")
            .HasComment("Amount paid to acquire the vehicle.");

        entity.Property(e => e.PurchasedFrom)
            .HasMaxLength(200)
            .HasComment(
                "Name of the dealership, seller, auction, fleet provider, or other source from which the vehicle was acquired.");

        entity.Property(e => e.IsPurchasedNew)
            .HasComment("Indicates whether the vehicle was purchased new or used.");

        entity.Property(e => e.Notes)
            .HasColumnType("text")
            .HasComment("Internal notes and remarks regarding the vehicle.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the vehicle is active and available for service operations.");

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

        entity.HasAlternateKey(e => e.WarehouseId)
            .HasName("UQ_FgsVehicle_WarehouseId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsVehicle_TenantId_CompanyId_IsActive");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WarehouseId })
            .HasDatabaseName("IX_FgsVehicle_TenantId_CompanyId_WarehouseId");

        entity.HasOne<FgsWarehouse>()
            .WithMany()
            .HasForeignKey(e => e.WarehouseId)
            .HasConstraintName("FK_FgsVehicle_FgsWarehouse_WarehouseId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
