using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Company");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.CompanyGuid).IsRequired();
        builder.Property(e => e.CompanyNumber).IsRequired();
        builder.Property(e => e.BusinessTypeId).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.LegalName).HasMaxLength(300);
        builder.Property(e => e.Email).HasMaxLength(300);
        builder.Property(e => e.PhoneNumber).HasMaxLength(50);
        builder.Property(e => e.Website).HasMaxLength(500);
        builder.Property(e => e.TaxId).HasMaxLength(100);
        builder.Property(e => e.FullLogoUrl).HasMaxLength(1000);
        builder.Property(e => e.CompactLogoUrl).HasMaxLength(1000);
        builder.Property(e => e.IconLogoUrl).HasMaxLength(1000);
        builder.Property(e => e.FaviconUrl).HasMaxLength(1000);
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.UpdatedOn);

        builder.HasIndex(e => e.CompanyGuid).IsUnique();
        builder.HasIndex(e => e.TenantId).HasDatabaseName("ix_company_tenant");

        builder.HasMany(e => e.Users)
            .WithOne(e => e.Company)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Invites)
            .WithOne(e => e.Company)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
