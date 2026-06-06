using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupZoneConfiguration : IEntityTypeConfiguration<FgsSetupZone>
{
    public void Configure(EntityTypeBuilder<FgsSetupZone> entity)
    {
        entity.ToTable("FgsSetupZone");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupZone");
        entity.ToTable(t => t.HasCheckConstraint(
            "CK_FgsSetupZone_Code_Upper",
            "\"Code\" = UPPER(\"Code\")"));
    }
}
