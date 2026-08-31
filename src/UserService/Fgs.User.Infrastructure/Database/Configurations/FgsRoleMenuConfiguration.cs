using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsRoleMenuConfiguration : IEntityTypeConfiguration<FgsRoleMenu>
{
    public void Configure(EntityTypeBuilder<FgsRoleMenu> entity)
    {
        entity.ToTable(
            "FgsRoleMenu",
            t => t.HasComment(
                "Stores the menu items assigned to each role within a tenant company and defines which navigation items the role can access."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the role menu assignment.");
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Tenant that owns the role menu assignment.");
        entity.Property(e => e.CompanyId)
            .HasComment("Company within the tenant that owns the role menu assignment.");
        entity.Property(e => e.RoleId)
            .HasComment("Role that is granted access to the menu item.");
        entity.Property(e => e.MenuId)
            .HasComment("Global menu item that the role is allowed to access.");
        entity.Property(e => e.CreatedOn)
            .HasComment("Date and time the role menu assignment was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User or system that created the role menu assignment.");
        entity.Property(e => e.UpdatedOn)
            .HasComment("Date and time the role menu assignment was last modified.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User or system that last modified the role menu assignment.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the role currently has access to the menu item.");
        entity.Property(e => e.DisplayOrder)
            .HasColumnType("smallint")
            .HasDefaultValue((short)1)
            .HasComment("Controls the display order of the menu item for the role.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RoleId, e.MenuId })
            .IsUnique()
            .HasDatabaseName("IX_FgsRoleMenu_TenantId_CompanyId_RoleId_MenuId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RoleId, e.IsActive })
            .HasDatabaseName("IX_FgsRoleMenu_TenantId_CompanyId_RoleId_IsActive");

        entity.HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .HasConstraintName("FK_FgsRoleMenu_FgsRole")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
