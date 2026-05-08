using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class FSGSetupLocationTypeConfiguration : IEntityTypeConfiguration<FSGSetupLocationType>
{
    public void Configure(EntityTypeBuilder<FSGSetupLocationType> builder)
    {
        builder.ToTable("FSGSetupLocationType");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.Code).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.UpdatedOn);

        builder.HasIndex(e => e.Code).IsUnique();
    }
}
