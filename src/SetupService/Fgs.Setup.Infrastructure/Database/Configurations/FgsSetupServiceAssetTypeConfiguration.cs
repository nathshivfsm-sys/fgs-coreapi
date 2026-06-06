using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupServiceAssetTypeConfiguration : IEntityTypeConfiguration<FgsSetupServiceAssetType>
{
    public void Configure(EntityTypeBuilder<FgsSetupServiceAssetType> entity)
    {
        entity.ToTable("FgsSetupServiceAssetType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupServiceAssetType");
        entity.ToTable(t => t.HasCheckConstraint(
            "CK_FgsSetupServiceAssetType_Code_Upper",
            "\"Code\" = UPPER(\"Code\")"));
    }
}
