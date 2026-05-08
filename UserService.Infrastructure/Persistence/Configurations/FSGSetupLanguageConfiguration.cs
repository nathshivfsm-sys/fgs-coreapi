using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class FSGSetupLanguageConfiguration : IEntityTypeConfiguration<FSGSetupLanguage>
{
    public void Configure(EntityTypeBuilder<FSGSetupLanguage> builder)
    {
        builder.ToTable("FSGSetupLanguage");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.Code).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.CultureCode).HasMaxLength(50).IsRequired();
        builder.Property(e => e.IsDefault).IsRequired();
        builder.Property(e => e.SortOrder).IsRequired();
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.UpdatedOn);

        builder.HasIndex(e => e.Code).IsUnique();
    }
}
