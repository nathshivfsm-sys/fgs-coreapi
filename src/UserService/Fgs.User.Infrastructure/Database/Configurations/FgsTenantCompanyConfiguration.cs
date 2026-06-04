using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsTenantCompanyConfiguration : IEntityTypeConfiguration<FgsTenantCompany>
{
    public void Configure(EntityTypeBuilder<FgsTenantCompany> entity)
    {
        entity.ToTable("FgsTenantCompany");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyGuid).HasColumnOrder(2);
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyGuid });
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyNumber })
            .HasName("UX_Company_Tenant_CompanyNumber");
        entity.HasAlternateKey(e => new { e.TenantId, e.Code })
            .HasName("UX_Company_Tenant_Code");
        entity.Property(e => e.Code).HasMaxLength(100);
        entity.Property(e => e.Name).HasMaxLength(200);
        entity.Property(e => e.LegalName).HasMaxLength(300);
        entity.Property(e => e.Email).HasMaxLength(300);
        entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        entity.Property(e => e.Website).HasMaxLength(500);
        entity.Property(e => e.CompanySize).HasMaxLength(20);
        entity.Property(e => e.TaxId).HasMaxLength(100);
        entity.Property(e => e.FullLogoUrl).HasMaxLength(1000);
        entity.Property(e => e.CompactLogoUrl).HasMaxLength(1000);
        entity.Property(e => e.IconLogoUrl).HasMaxLength(1000);
        entity.Property(e => e.FaviconUrl).HasMaxLength(1000);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
