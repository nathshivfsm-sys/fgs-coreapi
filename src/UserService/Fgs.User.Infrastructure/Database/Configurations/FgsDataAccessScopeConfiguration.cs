using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsDataAccessScopeConfiguration : IEntityTypeConfiguration<FgsDataAccessScope>
{
    public void Configure(EntityTypeBuilder<FgsDataAccessScope> entity)
    {
        entity.ToTable(
            "FgsDataAccessScope",
            t => t.HasComment(
                "Stores one or more scope rules that define the records included in a Data Access profile."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.ScopeType)
            .HasMaxLength(50)
            .HasComment("Business entity used to restrict data, such as Company, BusinessUnit, Region, Warehouse, Technician or WorkOrder.");
        entity.Property(e => e.Operator)
            .HasMaxLength(20)
            .HasComment("Comparison operator used by the rule, such as ALL, IN, EQUALS, ASSIGNED_TO_CURRENT_USER or MANAGER_OF_CURRENT_USER.");
        entity.Property(e => e.ScopeValue)
            .HasMaxLength(255)
            .HasComment("Comparison value used by the rule. NULL when the operator does not require a value.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which scope rules are evaluated and displayed.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the scope rule was created.");
        entity.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("User or system that created the scope rule.");

        entity.HasIndex(e => e.FgsDataAccessId)
            .HasDatabaseName("IX_FgsDataAccessScope_FgsDataAccessId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsDataAccessScope_TenantId_CompanyId");
        entity.HasIndex(e => e.ScopeType)
            .HasDatabaseName("IX_FgsDataAccessScope_ScopeType");

        entity.HasOne(e => e.FgsDataAccess)
            .WithMany(d => d.Scopes)
            .HasForeignKey(e => e.FgsDataAccessId)
            .HasConstraintName("FK_FgsDataAccessScope_FgsDataAccess")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
