using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmReminderConfiguration : IEntityTypeConfiguration<CrmReminder>
{
    public void Configure(EntityTypeBuilder<CrmReminder> entity)
    {
        entity.ToTable(
            "CrmReminder",
            t =>
            {
                t.HasComment(
                    "Stores reminders assigned to users or roles for follow-up, review, approval, notification, and workflow activities.");
                t.HasCheckConstraint("CK_CrmReminder_StatusId", "\"StatusId\" IN (1, 2, 3)");
                t.HasCheckConstraint("CK_CrmReminder_PriorityId", "\"PriorityId\" IN (1, 2, 3, 4)");
                t.HasCheckConstraint(
                    "CK_CrmReminder_Entity",
                    "(\"EntityId\" IS NULL AND \"EntityValue\" IS NULL) OR (\"EntityId\" IS NOT NULL AND \"EntityValue\" IS NOT NULL)");
            });

        entity.HasKey(e => e.Id).HasName("PK_CrmReminder");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.EntityId).HasComment("Related entity identifier.");
        entity.Property(e => e.EntityValue).HasComment("Primary key value of the related business record.");
        entity.Property(e => e.PriorityId).HasDefaultValue((short)2)
            .HasComment("Priority. 1=Low, 2=Normal, 3=High, 4=Critical.");
        entity.Property(e => e.StatusId).HasDefaultValue((short)1)
            .HasComment("Status. 1=Open, 2=Completed, 3=Cancelled.");
        entity.Property(e => e.Subject).HasMaxLength(250).IsRequired().HasComment("Reminder subject.");
        entity.Property(e => e.ReminderText).HasColumnType("text").IsRequired()
            .HasComment("Reminder details, notes, instructions, or comments.");
        entity.Property(e => e.DueOn).HasColumnType("timestamptz").IsRequired()
            .HasComment("Date and time the reminder is due.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User identifier of the user who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User identifier of the user who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_CrmReminder_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.StatusId, e.DueOn })
            .HasDatabaseName("IX_CrmReminder_StatusId_DueOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EntityId, e.EntityValue })
            .HasDatabaseName("IX_CrmReminder_EntityId_EntityValue");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CreatedBy })
            .HasDatabaseName("IX_CrmReminder_CreatedBy");
    }
}
