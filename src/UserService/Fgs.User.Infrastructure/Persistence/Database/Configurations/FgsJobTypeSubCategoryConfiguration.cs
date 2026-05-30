using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsJobTypeSubCategoryConfiguration : IEntityTypeConfiguration<FgsJobTypeSubCategory>
{
    public void Configure(EntityTypeBuilder<FgsJobTypeSubCategory> entity)
    {
        entity.ToTable("FgsJobTypeSubCategory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.ConfigureTenantCompanySetupFk("FK_FgsJobTypeSubCategory_FgsTenantCompany_TenantId_CompanyId");
        entity.Property(e => e.SubCategoryCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(150);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SubCategoryCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsJobTypeSubCategory_Tenant_Company_SubCategoryCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsJobTypeSubCategory_Tenant_Company");
    }
}
