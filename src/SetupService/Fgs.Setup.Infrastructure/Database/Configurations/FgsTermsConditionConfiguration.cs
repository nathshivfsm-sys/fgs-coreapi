using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsTermsConditionConfiguration : IEntityTypeConfiguration<FgsTermsCondition>
{
    public void Configure(EntityTypeBuilder<FgsTermsCondition> entity)
    {
        entity.ToTable("FgsTermsCondition", t =>
        {
            t.HasComment(
                "Stores terms and conditions definitions and their versions for use across estimates, invoices, work authorizations, signatures, and other business entities.");
            t.HasCheckConstraint(
                "CK_FgsTermsCondition_VersionNumber",
                "\"VersionNumber\" > 0");
        });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Surrogate primary key.");
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Tenant that owns the terms and conditions.");
        entity.Property(e => e.CompanyId)
            .HasComment("Company within the tenant that owns the terms and conditions.");
        entity.Property(e => e.Code)
            .HasColumnType("text")
            .HasComment(
                "Code identifying the terms and conditions definition. Multiple versions can exist for the same code.");
        entity.Property(e => e.Name)
            .HasColumnType("text")
            .HasComment("Display name of the terms and conditions.");
        entity.Property(e => e.VersionNumber)
            .HasComment("Sequential version number of the terms and conditions.");
        entity.Property(e => e.TermsText)
            .HasColumnType("text")
            .HasComment("Complete terms and conditions text for this version.");
        entity.Property(e => e.CreatedOn)
            .HasComment("UTC timestamp when the record was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn)
            .HasComment("UTC timestamp when the record was last updated.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User or process that last updated the record.");
        entity.Property(e => e.IsActive)
            .HasComment("Indicates whether the terms and conditions version is active.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code, e.VersionNumber })
            .HasName("UQ_FgsTermsCondition");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code })
            .HasDatabaseName("IX_FgsTermsCondition_TenantId_CompanyId_Code");
    }
}
