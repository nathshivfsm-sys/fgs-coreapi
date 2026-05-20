using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsSetupServiceAssetManufacturerConfiguration : IEntityTypeConfiguration<FgsSetupServiceAssetManufacturer>
{
    public void Configure(EntityTypeBuilder<FgsSetupServiceAssetManufacturer> entity)
    {
        entity.ToTable("FgsSetupServiceAssetManufacturer");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.ConfigureTenantCompanySetupFk(
            "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompany_TenantId_CompanyId");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupServiceAssetManufacturer");
        entity.ToTable(t => t.HasCheckConstraint(
            "CK_FgsSetupServiceAssetManufacturer_Code_Upper",
            "\"Code\" = UPPER(\"Code\")"));
    }
}
