using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupLaborRateTypeConfiguration : IEntityTypeConfiguration<FgsSetupLaborRateType>
{
    public void Configure(EntityTypeBuilder<FgsSetupLaborRateType> entity)
    {
        entity.ToTable("FgsSetupLaborRateType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasName("UQ_FgsSetupLaborRateType_TenantId_CompanyId_Name");
        entity.Property(e => e.Name).HasColumnType("text");
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.SortOrder).HasDefaultValue(0);
        entity.Property(e => e.IsSystem).HasDefaultValue(false);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSetupLaborRateType_TenantId_CompanyId_IsActive");
    }
}
