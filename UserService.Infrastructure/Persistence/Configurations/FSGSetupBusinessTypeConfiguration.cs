using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class FSGSetupBusinessTypeConfiguration : IEntityTypeConfiguration<FSGSetupBusinessType>
{
    public void Configure(EntityTypeBuilder<FSGSetupBusinessType> builder)
    {
        builder.ToTable("FSGSetupBusinessType");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.Code).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.SortOrder).IsRequired();
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.UpdatedOn);

        builder.HasIndex(e => e.Code).IsUnique();

        builder.HasMany(e => e.Companies)
            .WithOne(e => e.BusinessType)
            .HasForeignKey(e => e.BusinessTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
