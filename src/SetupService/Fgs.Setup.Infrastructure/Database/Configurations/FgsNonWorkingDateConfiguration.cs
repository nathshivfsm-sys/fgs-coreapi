using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsNonWorkingDateConfiguration : IEntityTypeConfiguration<FgsNonWorkingDate>
{
    public void Configure(EntityTypeBuilder<FgsNonWorkingDate> entity)
    {
        entity.ToTable(
            "FgsNonWorkingDate",
            t => t.HasComment(
                "Stores tenant/company specific calendar dates on which normal business operations are not scheduled."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key identity of the non-working date record.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Tenant identifier owning this non-working date.");
        entity.Property(e => e.CompanyId)
            .HasComment("Company identifier within the tenant owning this non-working date.");

        entity.Property(e => e.NonWorkingDate)
            .HasColumnType("date")
            .IsRequired()
            .HasComment(
                "Calendar date on which the company does not operate under its normal working schedule.");

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment(
                "Name identifying the non-working date, such as New Year's Day, Thanksgiving, Company Holiday, or Emergency Closure.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the non-working date record was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User identifier that created the non-working date record.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the non-working date record was last updated.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User identifier that last updated the non-working date record.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment(
                "Indicates whether the non-working date is active and should be considered when determining business availability and scheduling.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.NonWorkingDate })
            .HasName("UQ_FgsNonWorkingDate_TenantId_CompanyId_NonWorkingDate");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsNonWorkingDate_TenantId_CompanyId_IsActive");
    }
}
