using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupPaymentTermConfiguration : IEntityTypeConfiguration<FgsSetupPaymentTerm>
{
    public void Configure(EntityTypeBuilder<FgsSetupPaymentTerm> entity)
    {
        entity.ToTable("FgsSetupPaymentTerm");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasName("UQ_FgsSetupPaymentTerm");
    }
}
