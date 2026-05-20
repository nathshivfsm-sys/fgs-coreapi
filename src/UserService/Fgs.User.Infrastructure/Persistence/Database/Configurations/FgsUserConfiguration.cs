using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsUserConfiguration : IEntityTypeConfiguration<FgsUser>
{
    public void Configure(EntityTypeBuilder<FgsUser> entity)
    {
        entity.ToTable("FgsUser");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => new { e.TenantId, e.Email })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
        entity.Property(e => e.Email).HasMaxLength(300);
        entity.Property(e => e.DisplayName).HasMaxLength(200);
        entity.Property(e => e.EntraObjectId).HasMaxLength(100);
        entity.Property(e => e.Role).HasConversion<string>().HasMaxLength(50);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne(e => e.Tenant)
            .WithMany()
            .HasForeignKey(e => e.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.Company)
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(c => new { c.TenantId, c.CompanyNumber })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
