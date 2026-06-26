using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsJobTypeCategoryConfiguration : IEntityTypeConfiguration<FgsJobTypeCategory>
{
    public void Configure(EntityTypeBuilder<FgsJobTypeCategory> entity)
    {
        entity.ToTable("FgsJobTypeCategory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.Property(e => e.CategoryCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(150);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CategoryCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsJobTypeCategory_Tenant_Company_CategoryCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsJobTypeCategory_Tenant_Company");
    }
}
