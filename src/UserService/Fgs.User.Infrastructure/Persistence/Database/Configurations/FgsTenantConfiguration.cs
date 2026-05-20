using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsTenantConfiguration : IEntityTypeConfiguration<FgsTenant>
{
    public void Configure(EntityTypeBuilder<FgsTenant> entity)
    {
        entity.ToTable("FgsTenant");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.HasIndex(e => e.TenantCode).IsUnique();
        entity.Property(e => e.TenantCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(200);
        entity.Property(e => e.LegalName).HasMaxLength(300);
        entity.Property(e => e.Email).HasMaxLength(300);
        entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        entity.Property(e => e.Website).HasMaxLength(500);
        entity.Property(e => e.TimeZone).HasMaxLength(100);
        entity.Property(e => e.DefaultCurrency).HasMaxLength(20);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
