using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsInventoryCategoryConfiguration : IEntityTypeConfiguration<FgsInventoryCategory>
{
    public void Configure(EntityTypeBuilder<FgsInventoryCategory> entity)
    {
        entity.ToTable("FgsInventoryCategory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.ConfigureTenantCompanySetupFk("FK_FgsInventoryCategory_FgsTenantCompany_TenantId_CompanyId");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.CategoryCode })
            .HasName("UQ_FgsInventoryCategory_TenantId_CompanyId_CategoryCode");
        entity.Property(e => e.CategoryCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(150);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsSystem).HasDefaultValue(false);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
