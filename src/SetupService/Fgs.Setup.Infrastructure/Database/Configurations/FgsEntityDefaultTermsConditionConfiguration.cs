using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsEntityDefaultTermsConditionConfiguration : IEntityTypeConfiguration<FgsEntityDefaultTermsCondition>
{
    public void Configure(EntityTypeBuilder<FgsEntityDefaultTermsCondition> entity)
    {
        entity.ToTable(
            "FgsEntityDefaultTermsCondition",
            t => t.HasComment(
                "Stores the default terms and conditions version assigned to each supported entity type for a tenant and company."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Surrogate primary key.");
        entity.ConfigureTenantCompanySetupColumns(
            includeTenantCompanyIndex: true,
            tenantCompanyIndexName: "IX_FgsEntityDefaultTermsCondition_TenantId_CompanyId");

        entity.Property(e => e.TenantId)
            .HasComment("Tenant that owns the entity terms and conditions configuration.");
        entity.Property(e => e.CompanyId)
            .HasComment(
                "Company within the tenant for which the default terms and conditions are configured.");
        entity.Property(e => e.EntityType)
            .HasColumnType("text")
            .HasComment(
                "Entity type to which the default terms and conditions version applies, such as Invoice, Estimate, WorkAuthorization, or Signature.");
        entity.Property(e => e.TermsConditionId)
            .HasComment(
                "Reference to the specific terms and conditions version that is the default for the entity type.");
        entity.Property(e => e.CreatedOn)
            .HasComment("UTC timestamp when the record was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn)
            .HasComment("UTC timestamp when the record was last updated.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User or process that last updated the record.");
        entity.Property(e => e.IsActive)
            .HasComment("Indicates whether the default entity terms and conditions mapping is active.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.EntityType })
            .HasName("UQ_FgsEntityDefaultTermsCondition");

        entity.HasIndex(e => e.TermsConditionId)
            .HasDatabaseName("IX_FgsEntityDefaultTermsCondition_TermsConditionId");

        entity.HasOne(e => e.TermsCondition)
            .WithMany()
            .HasForeignKey(e => e.TermsConditionId)
            .HasConstraintName("FK_FgsEntityDefaultTermsCondition_FgsTermsCondition_TermsConditionId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
