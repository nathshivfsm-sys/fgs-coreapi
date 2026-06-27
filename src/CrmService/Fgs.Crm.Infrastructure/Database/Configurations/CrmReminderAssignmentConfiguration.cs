using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmReminderAssignmentConfiguration : IEntityTypeConfiguration<CrmReminderAssignment>
{
    public void Configure(EntityTypeBuilder<CrmReminderAssignment> entity)
    {
        entity.ToTable(
            "CrmReminderAssignment",
            t =>
            {
                t.HasComment("Stores user and role assignments for reminders.");
                t.HasCheckConstraint(
                    "CK_CrmReminderAssignment_Assignee",
                    "(\"UserId\" IS NOT NULL AND \"RoleId\" IS NULL) OR (\"UserId\" IS NULL AND \"RoleId\" IS NOT NULL)");
            });

        entity.HasKey(e => e.Id).HasName("PK_CrmReminderAssignment");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.ReminderId).HasComment("Related reminder identifier.");
        entity.Property(e => e.UserId).HasComment("Assigned user identifier.");
        entity.Property(e => e.RoleId).HasComment("Assigned role identifier.");
        entity.Property(e => e.ResponseText).HasColumnType("text")
            .HasComment("Response or completion notes entered by the assignee.");
        entity.Property(e => e.CompletedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the assignment was completed.");
        entity.Property(e => e.CompletedByUserId).HasComment("User identifier of the user who completed the reminder.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User identifier of the user who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User identifier of the user who last updated the record.");

        entity.HasOne<CrmReminder>()
            .WithMany()
            .HasForeignKey(e => e.ReminderId)
            .HasConstraintName("FK_CrmReminderAssignment_CrmReminder_ReminderId")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_CrmReminderAssignment_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ReminderId })
            .HasDatabaseName("IX_CrmReminderAssignment_ReminderId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UserId })
            .HasFilter("\"UserId\" IS NOT NULL")
            .HasDatabaseName("IX_CrmReminderAssignment_UserId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RoleId })
            .HasFilter("\"RoleId\" IS NOT NULL")
            .HasDatabaseName("IX_CrmReminderAssignment_RoleId");
    }
}
