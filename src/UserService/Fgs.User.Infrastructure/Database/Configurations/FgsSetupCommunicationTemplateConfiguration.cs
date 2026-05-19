using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsSetupCommunicationTemplateConfiguration : IEntityTypeConfiguration<FgsSetupCommunicationTemplate>
{
    public void Configure(EntityTypeBuilder<FgsSetupCommunicationTemplate> entity)
    {
        entity.ToTable("FgsSetupCommunicationTemplate");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.ConfigureTenantCompanySetupFk(
            "FK_FgsSetupCommunicationTemplate_FgsTenantCompany_TenantId_CompanyId");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TemplateType, e.Code })
            .HasName("UQ_FgsSetupCommunicationTemplate");
    }
}
