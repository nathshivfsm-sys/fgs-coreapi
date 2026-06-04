using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPaymentMethodConfiguration : IEntityTypeConfiguration<FgsSetupPaymentMethod>
{
    public void Configure(EntityTypeBuilder<FgsSetupPaymentMethod> entity)
    {
        entity.ToTable("FgsSetupPaymentMethod");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.DisplayName })
            .HasName("UQ_FgsSetupPaymentMethod");
        entity.Property(e => e.DisplayName).HasColumnType("text");
        entity.Property(e => e.SortOrder).HasDefaultValue(0);
        entity.Property(e => e.IsMobileVisible).HasDefaultValue(true);
        entity.Property(e => e.IsCustomerPortalVisible).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSetupPaymentMethod_TenantId_CompanyId_IsActive");
    }
}
