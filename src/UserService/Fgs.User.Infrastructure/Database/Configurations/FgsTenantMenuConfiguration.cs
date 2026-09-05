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
            t =>
            {
                t.HasComment(
                    "Stores the menu items enabled for a company within a tenant based on the tenant subscription and available platform features.");
                t.HasCheckConstraint(
                    "CK_FgsTenantMenu_Name_NotEmpty",
                    "length(trim(\"Name\")) > 0");
                t.HasCheckConstraint(
                    "CK_FgsTenantMenu_MenuCode_NotEmpty",
                    "length(trim(\"MenuCode\")) > 0");
                t.HasCheckConstraint(
                    "CK_FgsTenantMenu_MenuType_NotEmpty",
                    "length(trim(\"MenuType\")) > 0");
            });

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
        entity.Property(e => e.MenuCode)
            .HasMaxLength(50)
            .HasComment("Unique system-defined code identifying the menu item (copied from global catalog).");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasComment("Display name of the menu item shown to users.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Description of the menu item and its purpose.");
        entity.Property(e => e.ParentMenuId)
            .HasComment(
                "Global parent menu id when this item is nested; NULL for top-level menus.");
        entity.Property(e => e.MenuType)
            .HasMaxLength(20)
            .HasComment("Defines the type of menu item, such as a menu group or navigable page.");
        entity.Property(e => e.Route)
            .HasMaxLength(255)
            .HasComment("Application route used to navigate to the menu item when applicable.");
        entity.Property(e => e.Icon)
            .HasMaxLength(100)
            .HasComment("UI icon identifier associated with the menu item.");
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
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.MenuCode })
            .IsUnique()
            .HasDatabaseName("IX_FgsTenantMenu_TenantId_CompanyId_MenuCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsTenantMenu_TenantId_CompanyId_IsActive");
    }
}
