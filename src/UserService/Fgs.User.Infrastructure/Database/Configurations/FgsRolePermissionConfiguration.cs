using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsRolePermissionConfiguration : IEntityTypeConfiguration<FgsRolePermission>
{
    public void Configure(EntityTypeBuilder<FgsRolePermission> entity)
    {
        entity.ToTable(
            "FgsRolePermission",
            t => t.HasComment("Assigns permissions to security roles within a company."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the permission was assigned to the role.");
        entity.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("User or system that assigned the permission to the role.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsRoleId, e.FgsPermissionId })
            .IsUnique()
            .HasDatabaseName("IX_FgsRolePermission_TenantId_CompanyId_Role_Permission");
        entity.HasIndex(e => e.FgsRoleId)
            .HasDatabaseName("IX_FgsRolePermission_FgsRoleId");
        entity.HasIndex(e => e.FgsPermissionId)
            .HasDatabaseName("IX_FgsRolePermission_FgsPermissionId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsRolePermission_TenantId_CompanyId");

        entity.HasOne(e => e.FgsRole)
            .WithMany()
            .HasForeignKey(e => e.FgsRoleId)
            .HasConstraintName("FK_FgsRolePermission_FgsRole")
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(e => e.FgsPermission)
            .WithMany()
            .HasForeignKey(e => e.FgsPermissionId)
            .HasConstraintName("FK_FgsRolePermission_FgsPermission")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
