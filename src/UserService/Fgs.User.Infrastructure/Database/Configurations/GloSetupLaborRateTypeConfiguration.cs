using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class GloSetupLaborRateTypeConfiguration : IEntityTypeConfiguration<GloSetupLaborRateType>
{
    public void Configure(EntityTypeBuilder<GloSetupLaborRateType> entity)
    {
        entity.ToTable("GloSetupLaborRateType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.Name).IsUnique();
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
