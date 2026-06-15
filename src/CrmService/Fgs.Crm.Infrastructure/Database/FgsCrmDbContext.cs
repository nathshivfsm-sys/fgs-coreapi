using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Infrastructure.Database.Configurations;
using Fgs.Crm.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Crm.Infrastructure.Database;

public sealed class FgsCrmDbContext(DbContextOptions<FgsCrmDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    public DbSet<CrmOutboxMessage> CrmOutboxMessages => Set<CrmOutboxMessage>();

    public DbSet<CrmLead> CrmLeads => Set<CrmLead>();

    public DbSet<CrmCustomer> CrmCustomers => Set<CrmCustomer>();

    public DbSet<CrmServiceLocation> CrmServiceLocations => Set<CrmServiceLocation>();

    public DbSet<CrmContact> CrmContacts => Set<CrmContact>();

    public DbSet<CrmEntityTag> CrmEntityTags => Set<CrmEntityTag>();

    public DbSet<CrmContactCommunication> CrmContactCommunications => Set<CrmContactCommunication>();

    public DbSet<CrmNote> CrmNotes => Set<CrmNote>();

    public DbSet<CrmReminder> CrmReminders => Set<CrmReminder>();

    public DbSet<CrmReminderAssignment> CrmReminderAssignments => Set<CrmReminderAssignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Crm);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsCrmDbContext).Assembly);
        FgsCrmDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
        ConfigureAuditActorColumns(modelBuilder);
    }

    private static void ConfigureAuditActorColumns(ModelBuilder modelBuilder)
    {
        const int maxLength = 100;
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var createdBy = entityType.FindProperty("CreatedBy");
            if (createdBy?.ClrType == typeof(string))
            {
                createdBy.SetMaxLength(maxLength);
            }

            var updatedBy = entityType.FindProperty("UpdatedBy");
            if (updatedBy?.ClrType == typeof(string))
            {
                updatedBy.SetMaxLength(maxLength);
            }
        }
    }
}
