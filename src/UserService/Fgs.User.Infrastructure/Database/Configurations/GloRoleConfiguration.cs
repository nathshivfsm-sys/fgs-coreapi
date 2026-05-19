using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class GloRoleConfiguration : IEntityTypeConfiguration<GloRole>
{
    public void Configure(EntityTypeBuilder<GloRole> entity)
    {
        entity.ToTable("GloRole");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.RoleCode).IsUnique();
        entity.Property(e => e.RoleCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(255);
        entity.Property(e => e.RoleLevel).HasMaxLength(20);
        entity.Property(e => e.SortOrder).HasColumnType("smallint");
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
    }
}
