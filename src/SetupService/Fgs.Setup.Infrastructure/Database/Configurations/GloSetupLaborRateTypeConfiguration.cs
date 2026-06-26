using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloSetupLaborRateTypeConfiguration : IEntityTypeConfiguration<GloSetupLaborRateType>
{
    public void Configure(EntityTypeBuilder<GloSetupLaborRateType> entity)
    {
        entity.ToTable("GloSetupLaborRateType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.HasAlternateKey(e => e.Name).HasName("UQ_GloSetupLaborRateType_Name");
        entity.Property(e => e.SortOrder).HasDefaultValue(0);
        entity.Property(e => e.IsSystem).HasDefaultValue(true);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
