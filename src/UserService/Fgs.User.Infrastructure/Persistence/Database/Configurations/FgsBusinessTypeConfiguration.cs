using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsBusinessTypeConfiguration : IEntityTypeConfiguration<FgsBusinessType>
{
    public void Configure(EntityTypeBuilder<FgsBusinessType> entity)
    {
        entity.ToTable("FgsBusinessType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.ConfigureTenantCompanySetupFk("FK_FgsBusinessType_FgsTenantCompany_TenantId_CompanyId");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsBusinessType_TenantId_CompanyId_Code");
        entity.Property(e => e.Code).HasMaxLength(100);
        entity.Property(e => e.Name).HasMaxLength(200);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsBusinessType_TenantId_CompanyId_IsActive");
    }
}
