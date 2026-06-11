using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupServiceAgreementTemplateCoverageConfiguration
    : IEntityTypeConfiguration<FgsSetupServiceAgreementTemplateCoverage>
{
    public void Configure(EntityTypeBuilder<FgsSetupServiceAgreementTemplateCoverage> entity)
    {
        entity.ToTable(
            "FgsSetupServiceAgreementTemplateCoverage",
            t =>
            {
                t.HasComment(
                    "Stores included or excluded coverage items for a service agreement template.");
                t.HasCheckConstraint(
                    "CK_FgsSetupServiceAgreementTemplateCoverage_CoverageTypeCode",
                    "\"CoverageTypeCode\" IN ('INCLUDE','EXCLUDE')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();

        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.Property(e => e.ServiceAgreementTemplateId)
            .HasComment("Service agreement template that this coverage item belongs to.");

        entity.Property(e => e.CoverageTypeCode)
            .HasMaxLength(20)
            .IsRequired()
            .HasComment("INCLUDE or EXCLUDE.");
        entity.Property(e => e.Description).HasColumnType("text").IsRequired();
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementTemplateId })
            .HasDatabaseName("IX_FgsSetupServiceAgreementTemplateCoverage_TenantId_CompanyId_TemplateId");

        entity.HasOne<FgsSetupServiceAgreementTemplate>()
            .WithMany()
            .HasForeignKey(e => e.ServiceAgreementTemplateId)
            .HasConstraintName("FK_FgsSetupServiceAgreementTemplateCoverage_ServiceAgreementTemplateId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
