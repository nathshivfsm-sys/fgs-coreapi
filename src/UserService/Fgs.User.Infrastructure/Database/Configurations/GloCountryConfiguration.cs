using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class GloCountryConfiguration : IEntityTypeConfiguration<GloCountry>
{
    public void Configure(EntityTypeBuilder<GloCountry> entity)
    {
        entity.ToTable("GloCountry");
        entity.HasKey(e => e.CountryCode);
        entity.Property(e => e.CountryCode).HasMaxLength(2);
        entity.Property(e => e.CountryName).HasMaxLength(100);
        entity.Property(e => e.CurrencyCode).HasMaxLength(3);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
    }
}
