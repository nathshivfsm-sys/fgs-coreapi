using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsJobTypeAccountingConfiguration : IEntityTypeConfiguration<FgsJobTypeAccounting>
{
    public void Configure(EntityTypeBuilder<FgsJobTypeAccounting> entity)
    {
        entity.ToTable("FgsJobTypeAccounting", t =>
            t.HasComment(
                "Stores optional accounting configuration that applies at the job type level independently of billing category, including Accounts Receivable and Discount GL account mappings."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key identity of the job type accounting record.");

        entity.Property(e => e.TenantId)
            .HasComment("Tenant identifier owning this job type accounting configuration.");

        entity.Property(e => e.CompanyId)
            .HasComment("Company identifier within the tenant owning this job type accounting configuration.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.JobTypeId })
            .HasName("UQ_FgsJobTypeAccounting_TenantId_CompanyId_JobTypeId");

        entity.Property(e => e.JobTypeId)
            .HasComment("Identifier of the job type to which the accounting configuration applies.");

        entity.Property(e => e.ArAccountId)
            .HasComment(
                "Optional GL account used to record accounts receivable for transactions associated with the job type. This account is independent of billing category.");

        entity.Property(e => e.DiscountAccountId)
            .HasComment(
                "Optional GL account used to record discounts associated with the job type. This account is independent of billing category.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the job type accounting configuration was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User identifier that created the job type accounting configuration.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the job type accounting configuration was last updated.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User identifier that last updated the job type accounting configuration.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsJobTypeAccounting_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobTypeId })
            .HasDatabaseName("IX_FgsJobTypeAccounting_JobTypeId");

        entity.HasOne<FgsJobType>()
            .WithMany()
            .HasForeignKey(e => e.JobTypeId)
            .HasConstraintName("FK_FgsJobTypeAccounting_FgsJobType")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
