using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class FgsPriceBookItemConfiguration : IEntityTypeConfiguration<FgsPriceBookItem>
{
    public void Configure(EntityTypeBuilder<FgsPriceBookItem> entity)
    {
        entity.ToTable(
            "FgsPriceBookItem",
            t => t.HasComment(
                "Defines the inventory, non-inventory, and free-form items that make up a price book service."));

        entity.HasKey(e => e.Id).HasName("PK_FgsPriceBookItem");
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key identifier of the price book item.");

        entity.Property(e => e.TenantId)
            .HasComment("Tenant identifier owning the record.");
        entity.Property(e => e.CompanyId)
            .HasComment("Company identifier owning the record.");
        entity.Property(e => e.PriceBookId).IsRequired()
            .HasComment("Reference to the parent price book.");
        entity.Property(e => e.InventoryItemId)
            .HasComment("Identifier of the inventory or non-inventory item. No database foreign key is enforced because the inventory module resides in a separate schema.");
        entity.Property(e => e.ItemCode).HasMaxLength(50)
            .HasComment("Business code of the selected item. Stored as a snapshot for reporting and historical consistency.");
        entity.Property(e => e.ItemDescription).HasMaxLength(500).IsRequired()
            .HasComment("Description of the item as it should appear within the price book.");
        entity.Property(e => e.Quantity).HasPrecision(18, 4).HasDefaultValue(1m).IsRequired()
            .HasComment("Default quantity required to perform the service.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1).IsRequired()
            .HasComment("Controls the display order of items within the price book.");
        entity.Property(e => e.Notes).HasColumnType("text")
            .HasComment("Optional notes or installation instructions.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100)
            .HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100)
            .HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsPriceBookItem_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PriceBookId })
            .HasDatabaseName("IX_FgsPriceBookItem_TenantId_CompanyId_PriceBookId");
    }
}
