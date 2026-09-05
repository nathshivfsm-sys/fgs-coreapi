using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloMenuConfiguration : IEntityTypeConfiguration<GloMenu>
{
    public void Configure(EntityTypeBuilder<GloMenu> entity)
    {
        entity.ToTable("GloMenu", t =>
        {
            t.HasComment(
                "Global master definition of application menus and navigation items available across the FSM platform.");
            t.HasCheckConstraint(
                "CK_GloMenu_Name_NotEmpty",
                "length(trim(\"Name\")) > 0");
            t.HasCheckConstraint(
                "CK_GloMenu_MenuCode_NotEmpty",
                "length(trim(\"MenuCode\")) > 0");
        });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the menu item.");

        entity.HasAlternateKey(e => e.MenuCode).HasName("UX_GloMenu_MenuCode");

        entity.Property(e => e.MenuCode)
            .HasMaxLength(50)
            .HasComment("Unique system-defined code identifying the menu item.");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasComment("Display name of the menu item shown to users.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Description of the menu item and its purpose.");
        entity.Property(e => e.ParentMenuId)
            .HasComment(
                "References the parent menu item when this menu is a child item; NULL for top-level menus.");
        entity.Property(e => e.MenuType)
            .HasMaxLength(20)
            .HasComment("Defines the type of menu item, such as a menu group or navigable page.");
        entity.Property(e => e.Route)
            .HasMaxLength(255)
            .HasComment("Application route used to navigate to the menu item when applicable.");
        entity.Property(e => e.Icon)
            .HasMaxLength(100)
            .HasComment("UI icon identifier associated with the menu item.");
        entity.Property(e => e.SortOrder)
            .HasColumnType("smallint")
            .HasDefaultValue((short)0)
            .HasComment("Determines the display order of the menu item within its parent menu.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment(
                "Indicates whether the menu item is currently active and available for tenant configuration.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())")
            .HasComment("UTC timestamp when the menu item was created.");

        entity.HasOne(e => e.ParentMenu)
            .WithMany(e => e.ChildMenus)
            .HasForeignKey(e => e.ParentMenuId)
            .HasConstraintName("FK_GloMenu_ParentMenu")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
