using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloSetupDescriptionTypeConfiguration : IEntityTypeConfiguration<GloSetupDescriptionType>
{
    public void Configure(EntityTypeBuilder<GloSetupDescriptionType> entity)
    {
        entity.ToTable("GloSetupDescriptionType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        entity.HasAlternateKey(e => e.Code).HasName("UQ_GloSetupDescriptionType_Code");
        entity.Property(e => e.Code).HasMaxLength(100);
        entity.Property(e => e.Name).HasMaxLength(200);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
