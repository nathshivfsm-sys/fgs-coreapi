using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsUserRoleConfiguration : IEntityTypeConfiguration<FgsUserRole>
{
    public void Configure(EntityTypeBuilder<FgsUserRole> entity)
    {
        entity.ToTable("FgsUserRole");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId });
        entity.HasIndex(e => e.GloRoleId);
        entity.HasIndex(e => e.FgsRoleId);
        entity.HasIndex(e => new { e.UserId, e.GloRoleId })
            .IsUnique()
            .HasFilter("\"GloRoleId\" IS NOT NULL");
        entity.HasIndex(e => new { e.UserId, e.FgsRoleId })
            .IsUnique()
            .HasFilter("\"FgsRoleId\" IS NOT NULL");
        entity.ToTable(t => t.HasCheckConstraint(
            "CK_FgsUserRole_OnlyOneRole",
            "(\"GloRoleId\" IS NOT NULL AND \"FgsRoleId\" IS NULL) OR (\"GloRoleId\" IS NULL AND \"FgsRoleId\" IS NOT NULL)"));
        entity.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.GloRole)
            .WithMany()
            .HasForeignKey(e => e.GloRoleId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.FgsRole)
            .WithMany()
            .HasForeignKey(e => e.FgsRoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
