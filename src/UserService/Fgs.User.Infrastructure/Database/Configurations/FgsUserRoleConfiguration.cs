using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsUserRoleConfiguration : IEntityTypeConfiguration<FgsUserRole>
{
    public void Configure(EntityTypeBuilder<FgsUserRole> entity)
    {
        entity.ToTable(
            "FgsUserRole",
            t => t.HasComment("Assigns one or more security roles to users within a company."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the role assignment was created.");
        entity.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("User or system that assigned the role.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UserId, e.FgsRoleId })
            .IsUnique()
            .HasDatabaseName("IX_FgsUserRole_TenantId_CompanyId_UserId_FgsRoleId");
        entity.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_FgsUserRole_UserId");
        entity.HasIndex(e => e.FgsRoleId)
            .HasDatabaseName("IX_FgsUserRole_FgsRoleId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsUserRole_TenantId_CompanyId");

        entity.HasOne(e => e.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("FK_FgsUserRole_FgsUser_UserId")
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(e => e.FgsRole)
            .WithMany()
            .HasForeignKey(e => e.FgsRoleId)
            .HasConstraintName("FK_FgsUserRole_FgsRole_FgsRoleId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
