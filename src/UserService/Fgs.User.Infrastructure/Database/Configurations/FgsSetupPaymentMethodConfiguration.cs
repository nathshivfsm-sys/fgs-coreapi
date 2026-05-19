using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsSetupPaymentMethodConfiguration : IEntityTypeConfiguration<FgsSetupPaymentMethod>
{
    public void Configure(EntityTypeBuilder<FgsSetupPaymentMethod> entity)
    {
        entity.ToTable("FgsSetupPaymentMethod");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.ConfigureTenantCompanySetupFk("FK_FgsSetupPaymentMethod_FgsTenantCompany_TenantId_CompanyId");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.GloPaymentMethodTypeId })
            .HasName("UQ_FgsSetupPaymentMethod");
        entity.HasOne<GloPaymentMethodType>()
            .WithMany()
            .HasForeignKey(e => e.GloPaymentMethodTypeId)
            .HasConstraintName("FK_FgsSetupPaymentMethod_GloPayType")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
