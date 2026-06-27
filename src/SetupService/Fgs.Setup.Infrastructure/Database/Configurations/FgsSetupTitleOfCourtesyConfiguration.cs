using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupTitleOfCourtesyConfiguration : IEntityTypeConfiguration<FgsSetupTitleOfCourtesy>
{
    public void Configure(EntityTypeBuilder<FgsSetupTitleOfCourtesy> entity)
    {
        entity.ToTable("FgsSetupTitleOfCourtesy");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupTitleOfCourtesy");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SortOrder })
            .HasDatabaseName("IX_FgsSetupTitleOfCourtesy_SortOrder");
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsSetupTitleOfCourtesy_Code_Upper",
                "\"Code\" = UPPER(\"Code\")");
            t.HasCheckConstraint(
                "CK_FgsSetupTitleOfCourtesy_SortOrder",
                "\"SortOrder\" >= 0");
        });
    }
}
