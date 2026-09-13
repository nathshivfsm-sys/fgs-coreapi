using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloRolePermissionConfiguration : IEntityTypeConfiguration<GloRolePermission>
{
    public void Configure(EntityTypeBuilder<GloRolePermission> entity)
    {
        entity.ToTable(
            "GloRolePermission",
            t => t.HasComment(
                "Global default mapping of standard roles to permissions used to seed tenant role permission assignments during onboarding."));

        entity.HasKey(e => new { e.RoleId, e.PermissionId });

        entity.Property(e => e.RoleId)
            .HasColumnType("smallint")
            .HasComment("References the global standard role to which the permission is assigned.");
        entity.Property(e => e.PermissionId)
            .HasComment("References the global permission assigned to the role.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment(
                "Indicates whether this default role-to-permission assignment is active and should be included when seeding tenant role permission assignments.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())")
            .HasComment("UTC timestamp when the role-to-permission assignment was created.");

        entity.HasIndex(e => e.PermissionId).HasDatabaseName("IX_GloRolePermission_PermissionId");

        entity.HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .HasConstraintName("FK_GloRolePermission_Role")
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(e => e.Permission)
            .WithMany()
            .HasForeignKey(e => e.PermissionId)
            .HasConstraintName("FK_GloRolePermission_Permission")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
