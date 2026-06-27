using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloRoleConfiguration : IEntityTypeConfiguration<GloRole>
{
    public void Configure(EntityTypeBuilder<GloRole> entity)
    {
        entity.ToTable("GloRole", t =>
        {
            t.HasCheckConstraint(
                "CK_GloRole_RoleCode_NotEmpty",
                "length(trim(\"RoleCode\")) > 0");
            t.HasCheckConstraint(
                "CK_GloRole_Name_NotEmpty",
                "length(trim(\"Name\")) > 0");
        });
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();
        entity.HasAlternateKey(e => e.RoleCode).HasName("UX_GloRole_RoleCode");
        entity.Property(e => e.RoleCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(255);
        entity.Property(e => e.RoleLevel).HasMaxLength(20);
        entity.Property(e => e.IsAssignable).HasDefaultValue(true);
        entity.Property(e => e.IsSystemRole).HasDefaultValue(false);
        entity.Property(e => e.SortOrder)
            .HasColumnType("smallint")
            .HasDefaultValue((short)0);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
