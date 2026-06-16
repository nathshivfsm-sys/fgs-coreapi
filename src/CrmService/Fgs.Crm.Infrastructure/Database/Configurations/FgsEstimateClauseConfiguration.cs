using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateClauseConfiguration : IEntityTypeConfiguration<FgsEstimateClause>
{
    public void Configure(EntityTypeBuilder<FgsEstimateClause> entity)
    {
        entity.ToTable(
            "FgsEstimateClause",
            t =>
            {
                t.HasComment(
                    "Stores reusable estimate clauses that may be used across estimates and estimate templates.");
                t.HasCheckConstraint("CK_FgsEstimateClause_DisplayOrder", "\"DisplayOrder\" > 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.ClauseTypeId).HasComment(
            "Clause type such as Inclusion, Exclusion, or Terms and Conditions.");
        entity.Property(e => e.ClauseName).HasMaxLength(255).IsRequired()
            .HasComment("User-friendly clause name.");
        entity.Property(e => e.ClauseText).HasColumnType("text").IsRequired()
            .HasComment("Customer-facing clause text displayed on estimate documents.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Default display order.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the clause is available for use.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User or process that last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ClauseTypeId, e.ClauseName })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateClause_TenantId_CompanyId_ClauseTypeId_ClauseName");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateClause_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ClauseTypeId })
            .HasDatabaseName("IX_FgsEstimateClause_TenantId_CompanyId_ClauseTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsEstimateClause_TenantId_CompanyId_DisplayOrder");
    }
}
