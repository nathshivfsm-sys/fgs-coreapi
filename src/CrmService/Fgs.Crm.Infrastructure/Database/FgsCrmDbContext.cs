using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Infrastructure.Database.Configurations;
using Fgs.Crm.Infrastructure.Database.Schemas;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Crm.Infrastructure.Database;

public sealed class FgsCrmDbContext : FgsTenantFilteredDbContext
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public FgsCrmDbContext(
        DbContextOptions<FgsCrmDbContext> options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options, tenantContextAccessor)
    {
    }

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

    public DbSet<FgsEstimateFlavor> FgsEstimateFlavors => Set<FgsEstimateFlavor>();

    public DbSet<FgsEstimateStatus> FgsEstimateStatuses => Set<FgsEstimateStatus>();

    public DbSet<FgsEstimateTemplateCategory> FgsEstimateTemplateCategories => Set<FgsEstimateTemplateCategory>();

    public DbSet<FgsEstimate> FgsEstimates => Set<FgsEstimate>();

    public DbSet<FgsEstimateOption> FgsEstimateOptions => Set<FgsEstimateOption>();

    public DbSet<FgsEstimateOptionLine> FgsEstimateOptionLines => Set<FgsEstimateOptionLine>();

    public DbSet<FgsEstimateOptionTemplate> FgsEstimateOptionTemplates => Set<FgsEstimateOptionTemplate>();

    public DbSet<FgsEstimateClause> FgsEstimateClauses => Set<FgsEstimateClause>();

    public DbSet<FgsEstimateClauseItem> FgsEstimateClauseItems => Set<FgsEstimateClauseItem>();

    public DbSet<FgsEstimateTemplate> FgsEstimateTemplates => Set<FgsEstimateTemplate>();

    public DbSet<FgsEstimateTemplateOption> FgsEstimateTemplateOptions => Set<FgsEstimateTemplateOption>();

    public DbSet<FgsEstimateTemplateOptionLine> FgsEstimateTemplateOptionLines => Set<FgsEstimateTemplateOptionLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Crm);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsCrmDbContext).Assembly);
        FgsCrmDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
        FgsCrmDbContextConfigurationExtensions.ConfigureAuditActorColumns(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
    }
}
