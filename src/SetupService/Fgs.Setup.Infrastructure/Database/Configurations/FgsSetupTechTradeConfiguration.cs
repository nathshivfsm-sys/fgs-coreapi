using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupTechTradeConfiguration : IEntityTypeConfiguration<FgsSetupTechTrade>
{
    public void Configure(EntityTypeBuilder<FgsSetupTechTrade> entity)
    {
        entity.ToTable("FgsSetupTechTrade");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TradeCode })
            .HasName("UQ_FgsSetupTechTrade");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SortOrder })
            .HasDatabaseName("IX_FgsSetupTechTrade_SortOrder");
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsSetupTechTrade_TradeCode_Upper",
                "\"TradeCode\" = UPPER(\"TradeCode\")");
            t.HasCheckConstraint(
                "CK_FgsSetupTechTrade_SortOrder",
                "\"SortOrder\" >= 0");
        });
    }
}
