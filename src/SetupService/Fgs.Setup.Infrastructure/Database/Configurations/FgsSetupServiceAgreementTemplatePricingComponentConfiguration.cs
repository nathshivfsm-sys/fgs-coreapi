using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupServiceAgreementTemplatePricingComponentConfiguration
    : IEntityTypeConfiguration<FgsSetupServiceAgreementTemplatePricingComponent>
{
    public void Configure(EntityTypeBuilder<FgsSetupServiceAgreementTemplatePricingComponent> entity)
    {
        entity.ToTable(
            "FgsSetupServiceAgreementTemplatePricingComponent",
            t =>
            {
                t.HasComment(
                    "Stores pricing components included in a service agreement template. " +
                    "Rows are a snapshot copied from FgsSetupServiceAgreementPricingComponent when the template is configured and become independent of the master pricing component.");
                t.HasCheckConstraint(
                    "CK_FgsSetupServiceAgreementTemplatePricingComponent_Amount",
                    "\"Amount\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();

        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.Property(e => e.ServiceAgreementTemplateId)
            .HasComment("Service agreement template that includes this pricing component snapshot.");

        entity.Property(e => e.PricingComponentCode).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.Amount).HasPrecision(18, 2).IsRequired();
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasAlternateKey(e => new
        {
            e.TenantId,
            e.CompanyId,
            e.ServiceAgreementTemplateId,
            e.PricingComponentCode,
        })
            .HasName("UQ_FgsSetupServiceAgreementTemplatePricingComponent_TenantId_CompanyId_TemplateId_ComponentCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementTemplateId })
            .HasDatabaseName("IX_FgsSetupServiceAgreementTemplatePricingComponent_TenantId_CompanyId_TemplateId");

        entity.HasOne<FgsSetupServiceAgreementTemplate>()
            .WithMany()
            .HasForeignKey(e => e.ServiceAgreementTemplateId)
            .HasConstraintName("FK_FgsSetupServiceAgreementTemplatePricingComponent_ServiceAgreementTemplateId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
