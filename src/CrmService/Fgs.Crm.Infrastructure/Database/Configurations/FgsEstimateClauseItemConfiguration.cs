using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateClauseItemConfiguration : IEntityTypeConfiguration<FgsEstimateClauseItem>
{
    public void Configure(EntityTypeBuilder<FgsEstimateClauseItem> entity)
    {
        entity.ToTable(
            "FgsEstimateClauseItem",
            t =>
            {
                t.HasComment(
                    "Stores estimate-specific clause snapshots. Changes to the clause library do not affect existing estimates.");
                t.HasCheckConstraint("CK_FgsEstimateClauseItem_DisplayOrder", "\"DisplayOrder\" > 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.EstimateId).HasComment("Parent estimate.");
        entity.Property(e => e.ClauseId).HasComment("Source clause from crm.FgsEstimateClause.");
        entity.Property(e => e.ClauseTypeId).HasComment(
            "Snapshot of clause type such as Inclusion, Exclusion, or Terms and Conditions.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Controls display sequence on estimate documents.");
        entity.Property(e => e.ClauseName).HasMaxLength(255).IsRequired()
            .HasComment("Snapshot of clause name at the time it was added to the estimate.");
        entity.Property(e => e.ClauseText).HasColumnType("text").IsRequired()
            .HasComment("Snapshot of clause text at the time it was added to the estimate.");
        entity.Property(e => e.ShowOnProposal).HasDefaultValue(true)
            .HasComment("Indicates whether the clause should be displayed on customer-facing proposal documents.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User or process that last updated the record.");

        entity.HasOne<FgsEstimate>()
            .WithMany()
            .HasForeignKey(e => e.EstimateId)
            .HasConstraintName("FK_FgsEstimateClauseItem_Estimate")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<FgsEstimateClause>()
            .WithMany()
            .HasForeignKey(e => e.ClauseId)
            .HasConstraintName("FK_FgsEstimateClauseItem_Clause")
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateClauseItem_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateId })
            .HasDatabaseName("IX_FgsEstimateClauseItem_TenantId_CompanyId_EstimateId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ClauseTypeId })
            .HasDatabaseName("IX_FgsEstimateClauseItem_TenantId_CompanyId_ClauseTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsEstimateClauseItem_TenantId_CompanyId_DisplayOrder");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateId, e.DisplayOrder })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateClauseItem_TenantId_CompanyId_EstimateId_DisplayOrder");
    }
}
