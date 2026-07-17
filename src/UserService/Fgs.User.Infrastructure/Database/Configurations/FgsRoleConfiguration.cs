using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsRoleConfiguration : IEntityTypeConfiguration<FgsRole>
{
    public void Configure(EntityTypeBuilder<FgsRole> entity)
    {
        entity.ToTable(
            "FgsRole",
            t => t.HasComment(
                "Stores built-in platform roles and company-defined custom roles used by the authorization system."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.ParentRoleId)
            .HasComment("Original role from which this role was cloned. NULL for built-in roles or roles created from scratch.");
        entity.Property(e => e.RoleCode)
            .HasMaxLength(50)
            .HasComment("Unique system identifier for the role. Used internally by the application and should not be editable after creation.");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasComment("Display name shown to administrators and users.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the purpose of the role.");
        entity.Property(e => e.IsBuiltIn)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the role is provided by the platform. Built-in roles cannot be edited, deleted, or deactivated but may be cloned.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the display order of roles within the user interface.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the role is available for assignment. Built-in roles should always remain active.");
        entity.Property(e => e.CreatedOn)
            .HasComment("Date and time the role was created.");
        entity.Property(e => e.UpdatedOn)
            .HasComment("Date and time the role was last modified.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RoleCode })
            .IsUnique()
            .HasDatabaseName("IX_FgsRole_TenantId_CompanyId_RoleCode");
        entity.HasIndex(e => e.ParentRoleId)
            .HasDatabaseName("IX_FgsRole_ParentRoleId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsRole_TenantId_CompanyId_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsBuiltIn })
            .HasDatabaseName("IX_FgsRole_TenantId_CompanyId_IsBuiltIn");

        entity.HasOne(e => e.ParentRole)
            .WithMany(e => e.ChildRoles)
            .HasForeignKey(e => e.ParentRoleId)
            .HasConstraintName("FK_FgsRole_ParentRole")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
