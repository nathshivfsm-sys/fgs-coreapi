using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class TenantCompanyConfiguration : IEntityTypeConfiguration<TenantCompany>
{
    public void Configure(EntityTypeBuilder<TenantCompany> builder)
    {
        builder.ToTable("tenant_company");

        builder.HasKey(e => new { e.TenantId, e.CompanyId });

        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasIndex(e => e.TenantId).HasDatabaseName("ix_tenant_company_tenant");

        builder.HasMany(e => e.Users)
            .WithOne(e => e.TenantCompany)
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(e => new { e.TenantId, e.CompanyId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Invites)
            .WithOne(e => e.TenantCompany)
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(e => new { e.TenantId, e.CompanyId })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
