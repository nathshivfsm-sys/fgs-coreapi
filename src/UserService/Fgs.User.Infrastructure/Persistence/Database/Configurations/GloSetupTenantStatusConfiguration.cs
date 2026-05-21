using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloSetupTenantStatusConfiguration : IEntityTypeConfiguration<GloSetupTenantStatus>
{
    public void Configure(EntityTypeBuilder<GloSetupTenantStatus> entity)
    {
        entity.ToTable("GloSetupTenantStatus");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityAlwaysColumn();
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(500);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasColumnType("bigint");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedBy).HasColumnType("bigint");
        entity.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UX_GloSetupTenantStatus_Name");
    }
}
