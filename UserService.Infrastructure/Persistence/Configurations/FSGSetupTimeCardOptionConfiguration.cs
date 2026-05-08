using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class FSGSetupTimeCardOptionConfiguration : IEntityTypeConfiguration<FSGSetupTimeCardOption>
{
    public void Configure(EntityTypeBuilder<FSGSetupTimeCardOption> builder)
    {
        builder.ToTable("FSGSetupTimeCardOption");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.Code).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(2000);
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.UpdatedOn);

        builder.HasIndex(e => e.Code).IsUnique();
    }
}
