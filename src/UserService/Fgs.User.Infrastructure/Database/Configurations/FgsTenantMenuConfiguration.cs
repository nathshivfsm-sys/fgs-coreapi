using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsTenantMenuConfiguration : IEntityTypeConfiguration<FgsTenantMenu>
{
    public void Configure(EntityTypeBuilder<FgsTenantMenu> entity)
    {
        entity.ToTable(
            "FgsTenantMenu",
            t => t.HasComment(
                "Stores the menu items enabled for a company within a tenant based on the tenant subscription and available platform features."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the tenant menu assignment.");
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Tenant that owns the menu assignment.");
        entity.Property(e => e.CompanyId)
            .HasComment("Company within the tenant that receives the menu item.");
        entity.Property(e => e.MenuId)
            .HasComment("Global menu item assigned to the tenant company.");
        entity.Property(e => e.CreatedOn)
            .HasComment("Date and time the tenant menu assignment was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User or system that created the tenant menu assignment.");
        entity.Property(e => e.UpdatedOn)
            .HasComment("Date and time the tenant menu assignment was last modified.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User or system that last modified the tenant menu assignment.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the menu item is currently available to the tenant company.");
        entity.Property(e => e.DisplayOrder)
            .HasColumnType("smallint")
            .HasDefaultValue((short)1)
            .HasComment("Controls the display order of the menu item for the tenant company.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.MenuId })
            .IsUnique()
            .HasDatabaseName("IX_FgsTenantMenu_TenantId_CompanyId_MenuId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsTenantMenu_TenantId_CompanyId_IsActive");
    }
}
