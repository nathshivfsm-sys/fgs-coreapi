using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsInventoryLocationConfiguration : IEntityTypeConfiguration<FgsInventoryLocation>
{
    public void Configure(EntityTypeBuilder<FgsInventoryLocation> entity)
    {
        entity.ToTable(
            "FgsInventoryLocation",
            t =>
            {
                t.HasComment(
                    "Stores all inventory locations including warehouses, trucks, trailers, job sites, vendor locations and consignment locations.");
                t.HasCheckConstraint(
                    "CK_FgsInventoryLocation_InventoryLocationType",
                    "\"InventoryLocationType\" IN ('WAREHOUSE', 'TRUCK', 'TRAILER', 'JOBSITE', 'CONSIGNMENT', 'VENDOR')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn().HasComment("Primary key.");
        entity.ConfigureCatalogColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.InventoryLocationCode).HasMaxLength(50).IsRequired()
            .HasComment("Unique inventory location code.");
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired()
            .HasComment("Display name.");
        entity.Property(e => e.InventoryLocationType).HasMaxLength(30).IsRequired()
            .HasComment("WAREHOUSE, TRUCK, TRAILER, JOBSITE, CONSIGNMENT or VENDOR.");
        entity.Property(e => e.ParentInventoryLocationId)
            .HasComment("Optional parent inventory location.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Description or notes.");
        entity.Property(e => e.Address1).HasMaxLength(200).HasComment("Address line 1.");
        entity.Property(e => e.Address2).HasMaxLength(200).HasComment("Address line 2.");
        entity.Property(e => e.City).HasMaxLength(100).HasComment("City.");
        entity.Property(e => e.StateProvince).HasMaxLength(100).HasComment("State or province.");
        entity.Property(e => e.PostalCode).HasMaxLength(20).HasComment("Postal code.");
        entity.Property(e => e.Country).HasMaxLength(100).HasComment("Country.");
        entity.Property(e => e.ContactName).HasMaxLength(150).HasComment("Primary contact.");
        entity.Property(e => e.PhoneNumber).HasMaxLength(50).HasComment("Contact phone.");
        entity.Property(e => e.Email).HasMaxLength(255).HasComment("Contact email.");
        entity.Property(e => e.TextColor).HasMaxLength(20).HasComment("UI text color.");
        entity.Property(e => e.BackgroundColor).HasMaxLength(20).HasComment("UI background color.");
        entity.Property(e => e.DisplayIconFileId).HasComment("Display icon file identifier.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1).HasComment("Display order.");
        entity.Property(e => e.IsDefault).HasDefaultValue(false).HasComment("Default inventory location.");
        entity.Property(e => e.IsActive).HasComment("Active flag.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.InventoryLocationCode })
            .HasName("UQ_FgsInventoryLocation_TenantId_CompanyId_InventoryLocationCode");

        entity.HasOne<FgsInventoryLocation>()
            .WithMany()
            .HasForeignKey(e => e.ParentInventoryLocationId)
            .HasConstraintName("FK_FgsInventoryLocation_ParentInventoryLocation")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInventoryLocation_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryLocationType })
            .HasDatabaseName("IX_FgsInventoryLocation_TenantId_CompanyId_InventoryLocationType");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsInventoryLocation_TenantId_CompanyId_Name");
    }
}
