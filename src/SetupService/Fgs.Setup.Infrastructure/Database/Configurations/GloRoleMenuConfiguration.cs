using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloRoleMenuConfiguration : IEntityTypeConfiguration<GloRoleMenu>
{
    public void Configure(EntityTypeBuilder<GloRoleMenu> entity)
    {
        entity.ToTable(
            "GloRoleMenu",
            t => t.HasComment(
                "Global default mapping of standard roles to menu items used to seed tenant role menu assignments during onboarding."));

        entity.HasKey(e => new { e.RoleId, e.MenuId });

        entity.Property(e => e.RoleId)
            .HasColumnType("smallint")
            .HasComment("References the global standard role to which the menu item is assigned.");
        entity.Property(e => e.MenuId)
            .HasComment("References the global menu item assigned to the role.");
        entity.Property(e => e.SortOrder)
            .HasColumnType("smallint")
            .HasDefaultValue((short)0)
            .HasComment("Determines the display order of the menu item for the role.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment(
                "Indicates whether this default role-to-menu assignment is active and should be included when seeding tenant role menu assignments.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())")
            .HasComment("UTC timestamp when the role-to-menu assignment was created.");

        entity.HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .HasConstraintName("FK_GloRoleMenu_Role")
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(e => e.Menu)
            .WithMany()
            .HasForeignKey(e => e.MenuId)
            .HasConstraintName("FK_GloRoleMenu_Menu")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
