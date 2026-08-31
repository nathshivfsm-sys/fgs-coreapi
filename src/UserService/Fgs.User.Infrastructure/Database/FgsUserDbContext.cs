using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database.Configurations;
using Fgs.User.Infrastructure.Database.Schemas;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Database;

public class FgsUserDbContext : FgsTenantFilteredDbContext
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public FgsUserDbContext(
        DbContextOptions<FgsUserDbContext> options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options, tenantContextAccessor)
    {
    }

    public DbSet<FgsTenant> FgsTenants => Set<FgsTenant>();
    public DbSet<FgsTenantCompany> FgsTenantCompanies => Set<FgsTenantCompany>();
    public DbSet<FgsTenantServiceSetup> FgsTenantServiceSetups => Set<FgsTenantServiceSetup>();
    public DbSet<FgsTenantServiceAccountsSetup> FgsTenantServiceAccountsSetups => Set<FgsTenantServiceAccountsSetup>();
    public DbSet<FgsLocation> FgsLocations => Set<FgsLocation>();
    public DbSet<FgsUser> FgsUsers => Set<FgsUser>();
    public DbSet<FgsUserRole> FgsUserRoles => Set<FgsUserRole>();
    public DbSet<FgsRole> FgsRoles => Set<FgsRole>();
    public DbSet<FgsInvitation> FgsInvitations => Set<FgsInvitation>();
    public DbSet<FgsPermission> FgsPermissions => Set<FgsPermission>();
    public DbSet<FgsRolePermission> FgsRolePermissions => Set<FgsRolePermission>();
    public DbSet<FgsTenantMenu> FgsTenantMenus => Set<FgsTenantMenu>();
    public DbSet<FgsRoleMenu> FgsRoleMenus => Set<FgsRoleMenu>();
    public DbSet<FgsDataAccess> FgsDataAccesses => Set<FgsDataAccess>();
    public DbSet<FgsDataAccessScope> FgsDataAccessScopes => Set<FgsDataAccessScope>();
    public DbSet<FgsRoleDataAccess> FgsRoleDataAccesses => Set<FgsRoleDataAccess>();
    public DbSet<FgsPublicEndpoint> FgsPublicEndpoints => Set<FgsPublicEndpoint>();
    public DbSet<FgsApiEvent> FgsApiEvents => Set<FgsApiEvent>();
    public DbSet<FgsApiWebhook> FgsApiWebhooks => Set<FgsApiWebhook>();
    public DbSet<FgsApiWebhookSubscription> FgsApiWebhookSubscriptions => Set<FgsApiWebhookSubscription>();
    public DbSet<FgsApiClient> FgsApiClients => Set<FgsApiClient>();
    public DbSet<FgsApiSecret> FgsApiSecrets => Set<FgsApiSecret>();
    public DbSet<FgsApiRequestLog> FgsApiRequestLogs => Set<FgsApiRequestLog>();

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    public DbSet<TenantOutboxMessage> TenantOutboxMessages => Set<TenantOutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsUserDbContext).Assembly);
        EntitySchemaRegistry.ApplySchemas(modelBuilder);
        FgsUserDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
        ConfigureAuditActorColumns(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
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
