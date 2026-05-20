using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsSetupTechSkillLevelConfiguration : IEntityTypeConfiguration<FgsSetupTechSkillLevel>
{
    public void Configure(EntityTypeBuilder<FgsSetupTechSkillLevel> entity)
    {
        entity.ToTable("FgsSetupTechSkillLevel");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.ConfigureTenantCompanySetupFk("FK_FgsSetupTechSkillLevel_FgsTenantCompany_TenantId_CompanyId");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasName("UQ_FgsSetupTechSkillLevel");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SortOrder })
            .HasDatabaseName("IX_FgsSetupTechSkillLevel_SortOrder");
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsSetupTechSkillLevel_Code_Upper",
                "\"Code\" = UPPER(\"Code\")");
            t.HasCheckConstraint(
                "CK_FgsSetupTechSkillLevel_SortOrder",
                "\"SortOrder\" >= 0");
        });
    }
}
