using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsRoleConfiguration : IEntityTypeConfiguration<FgsRole>
{
    public void Configure(EntityTypeBuilder<FgsRole> entity)
    {
        entity.ToTable("FgsRole");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RoleCode }).IsUnique();
        entity.Property(e => e.RoleCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(255); entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
