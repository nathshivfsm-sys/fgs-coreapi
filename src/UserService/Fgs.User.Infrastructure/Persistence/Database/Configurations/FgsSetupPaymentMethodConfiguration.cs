using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsSetupPaymentMethodConfiguration : IEntityTypeConfiguration<FgsSetupPaymentMethod>
{
    public void Configure(EntityTypeBuilder<FgsSetupPaymentMethod> entity)
    {
        entity.ToTable("FgsSetupPaymentMethod");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.ConfigureTenantCompanySetupFk("FK_FgsSetupPaymentMethod_FgsTenantCompany_TenantId_CompanyId");
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
