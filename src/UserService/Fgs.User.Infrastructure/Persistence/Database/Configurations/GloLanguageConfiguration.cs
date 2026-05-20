using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloLanguageConfiguration : IEntityTypeConfiguration<GloLanguage>
{
    public void Configure(EntityTypeBuilder<GloLanguage> entity)
    {
        entity.ToTable("GloLanguage");
        entity.HasKey(e => e.LanguageCode);
        entity.Property(e => e.LanguageCode).HasMaxLength(5);
        entity.Property(e => e.LanguageName).HasMaxLength(100);
        entity.Property(e => e.CultureCode).HasMaxLength(10);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
    }
}
