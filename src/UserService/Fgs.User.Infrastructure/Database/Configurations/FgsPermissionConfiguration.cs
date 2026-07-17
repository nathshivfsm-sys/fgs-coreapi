using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsPermissionConfiguration : IEntityTypeConfiguration<FgsPermission>
{
    public void Configure(EntityTypeBuilder<FgsPermission> entity)
    {
        entity.ToTable(
            "FgsPermission",
            t => t.HasComment(
                "Master catalog of all permissions supported by the platform. Permissions are seeded by the application and assigned to security roles."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();

        entity.Property(e => e.PermissionCode)
            .HasMaxLength(100)
            .HasComment("Unique system identifier for the permission. Example: WORKORDER.CREATE.");
        entity.Property(e => e.Module)
            .HasMaxLength(50)
            .HasComment("Functional module that owns the permission. Example: Work Orders, Billing, CRM.");
        entity.Property(e => e.Resource)
            .HasMaxLength(50)
            .HasComment("Business resource protected by the permission. Example: WorkOrder, Invoice, Customer.");
        entity.Property(e => e.Action)
            .HasMaxLength(50)
            .HasComment("Operation allowed by the permission. Example: View, Create, Edit, Delete, Approve, Dispatch.");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasComment("User-friendly permission name displayed in the application.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the purpose of the permission.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the display order of permissions within the user interface.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the permission is available for assignment.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the permission was created.");

        entity.HasIndex(e => e.PermissionCode)
            .IsUnique()
            .HasDatabaseName("IX_FgsPermission_PermissionCode");
        entity.HasIndex(e => e.Module)
            .HasDatabaseName("IX_FgsPermission_Module");
        entity.HasIndex(e => e.Resource)
            .HasDatabaseName("IX_FgsPermission_Resource");
        entity.HasIndex(e => new { e.Module, e.Resource })
            .HasDatabaseName("IX_FgsPermission_Module_Resource");
    }
}
