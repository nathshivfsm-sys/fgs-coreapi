using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsBillingCategoryConfiguration : IEntityTypeConfiguration<FgsBillingCategory>
{
    public void Configure(EntityTypeBuilder<FgsBillingCategory> entity)
    {
        entity.ToTable("FgsBillingCategory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.ConfigureTenantCompanySetupFk("FK_FgsBillingCategory_FgsTenantCompany_TenantId_CompanyId");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.BillingCategoryType })
            .HasName("UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType");
        entity.Property(e => e.BillingCategoryType).HasMaxLength(2);
        entity.Property(e => e.BillingCategoryName).HasMaxLength(100);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsSystemDefined).HasDefaultValue(false);
        entity.Property(e => e.ShowToFieldTech).HasDefaultValue(true);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsBillingCategory_TenantId_CompanyId_IsActive");
    }
}
