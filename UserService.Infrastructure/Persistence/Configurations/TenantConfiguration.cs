using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenant");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.LegalName).HasMaxLength(300);
        builder.Property(e => e.Email).HasMaxLength(300);
        builder.Property(e => e.PhoneNumber).HasMaxLength(50);
        builder.Property(e => e.Website).HasMaxLength(500);
        builder.Property(e => e.TimeZone).HasMaxLength(100);
        builder.Property(e => e.DefaultCurrency).HasMaxLength(20);
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.UpdatedOn);

        builder.HasMany(e => e.Companies)
            .WithOne(e => e.Tenant)
            .HasForeignKey(e => e.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Users)
            .WithOne(e => e.Tenant)
            .HasForeignKey(e => e.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Invites)
            .WithOne(e => e.Tenant)
            .HasForeignKey(e => e.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
