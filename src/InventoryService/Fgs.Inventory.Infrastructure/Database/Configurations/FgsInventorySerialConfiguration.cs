using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsInventorySerialConfiguration : IEntityTypeConfiguration<FgsInventorySerial>
{
    public void Configure(EntityTypeBuilder<FgsInventorySerial> entity)
    {
        entity.ToTable(
            "FgsInventorySerial",
            t => t.HasComment(
                "Stores individual serialized inventory units and their current lifecycle status. Inventory movement history is maintained in FgsInventoryTransaction."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the serialized inventory unit.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Identifies the tenant that owns the serialized inventory unit.");
        entity.Property(e => e.CompanyId).HasComment("Identifies the company that owns the serialized inventory unit.");
        entity.Property(e => e.InventoryItemId)
            .HasComment("References the inventory item associated with this serialized inventory unit.");
        entity.Property(e => e.SerialNumber).HasMaxLength(200).IsRequired()
            .HasComment("Manufacturer or company-assigned serial number uniquely identifying the physical inventory unit.");
        entity.Property(e => e.InventorySerialStatus)
            .IsRequired()
            .HasDefaultValue(FgsInventorySerialStatus.Available)
            .HasComment("Current lifecycle status of the serialized inventory unit.");
        entity.Property(e => e.Notes).HasColumnType("text")
            .HasComment("Optional notes associated with the serialized inventory unit.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the serialized inventory unit was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100)
            .HasComment("User who created the serialized inventory unit.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time when the serialized inventory unit was last modified.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100)
            .HasComment("User who last modified the serialized inventory unit.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.InventoryItemId, e.SerialNumber })
            .HasName("UQ_FgsInventorySerial_TenantId_CompanyId_InventoryItemId_SerialNumber");

        entity.HasOne<FgsInventoryItem>()
            .WithMany()
            .HasForeignKey(e => e.InventoryItemId)
            .HasConstraintName("FK_FgsInventorySerial_FgsInventoryItem")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryItemId })
            .HasDatabaseName("IX_FgsInventorySerial_TenantId_CompanyId_InventoryItemId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventorySerialStatus })
            .HasDatabaseName("IX_FgsInventorySerial_TenantId_CompanyId_InventorySerialStatus");
    }
}
