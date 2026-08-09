using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsTruckStockTemplateConfiguration : IEntityTypeConfiguration<FgsTruckStockTemplate>
{
    public void Configure(EntityTypeBuilder<FgsTruckStockTemplate> entity)
    {
        entity.ToTable(
            "FgsTruckStockTemplate",
            t => t.HasComment(
                "Defines reusable truck stock templates that specify the desired inventory configuration for service vehicles. Templates are used during truck commissioning and synchronization and do not store or create inventory themselves."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the truck stock template.");
        entity.ConfigureCatalogColumns();

        entity.Property(e => e.TenantId)
            .HasComment("Identifies the tenant that owns this truck stock template.");
        entity.Property(e => e.CompanyId)
            .HasComment("Identifies the company that owns this truck stock template.");
        entity.Property(e => e.TemplateCode).HasMaxLength(100).IsRequired()
            .HasComment("Unique business code used to identify the truck stock template within a company.");
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired()
            .HasComment("User-friendly name of the truck stock template.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Optional description explaining the intended purpose or usage of the truck stock template.");
        entity.Property(e => e.IsActive)
            .HasComment("Indicates whether the truck stock template is available for use when commissioning or synchronizing truck inventory.");
        entity.Property(e => e.CreatedOn)
            .HasComment("Date and time when the truck stock template was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User who created the truck stock template.");
        entity.Property(e => e.UpdatedOn)
            .HasComment("Date and time when the truck stock template was last modified.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User who last modified the truck stock template.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TemplateCode })
            .HasName("UQ_FgsTruckStockTemplate_TenantId_CompanyId_TemplateCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsTruckStockTemplate_TenantId_CompanyId_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsTruckStockTemplate_IsActive");

        entity.HasMany(e => e.Items)
            .WithOne(e => e.TruckStockTemplate)
            .HasForeignKey(e => e.TruckStockTemplateId)
            .HasConstraintName("FK_FgsTruckStockTemplateItem_FgsTruckStockTemplate")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
