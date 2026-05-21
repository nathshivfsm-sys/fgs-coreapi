using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloCredentialCategoryConfiguration : IEntityTypeConfiguration<GloCredentialCategory>
{
    public void Configure(EntityTypeBuilder<GloCredentialCategory> entity)
    {
        entity.ToTable("GloCredentialCategory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.Code).IsUnique();
        entity.Property(e => e.Code).HasMaxLength(100);
        entity.Property(e => e.Name).HasMaxLength(200);
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
