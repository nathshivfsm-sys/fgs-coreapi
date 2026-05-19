using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class GloResolutionTypeConfiguration : IEntityTypeConfiguration<GloResolutionType>
{
    public void Configure(EntityTypeBuilder<GloResolutionType> entity)
    {
        entity.ToTable("GloResolutionType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.ResolutionTypeCode).HasMaxLength(50);
        entity.Property(e => e.ResolutionTypeName).HasMaxLength(200);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasAlternateKey(e => e.ResolutionTypeCode).HasName("UQ_GloResolutionType_Code");
    }
}
