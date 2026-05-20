using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloMasterEntityTypeConfiguration : IEntityTypeConfiguration<GloMasterEntityType>
{
    public void Configure(EntityTypeBuilder<GloMasterEntityType> entity)
    {
        entity.ToTable("GloMasterEntityType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.Code).HasMaxLength(100);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasAlternateKey(e => e.Code).HasName("UQ_GloMasterEntityType_Code");
    }
}
