using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsSetupGLBreakTechTradeConfiguration : IEntityTypeConfiguration<FgsSetupGLBreakTechTrade>
{
    public void Configure(EntityTypeBuilder<FgsSetupGLBreakTechTrade> entity)
    {
        entity.ToTable("FgsSetupGLBreakTechTrade");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.ConfigureTenantCompanySetupFk("FK_FgsSetupGLBreakTechTrade_FgsTenantCompany_TenantId_CompanyId");
        entity.HasIndex(e => new { e.FgsSetupGLBreakId, e.FgsSetupTechTradeId }).IsUnique();
        entity.HasOne(e => e.GLBreak)
            .WithMany(b => b.TechTrades)
            .HasForeignKey(e => e.FgsSetupGLBreakId)
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(e => e.TechTrade)
            .WithMany()
            .HasForeignKey(e => e.FgsSetupTechTradeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
