using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence;

public sealed class UserServiceDbContext : DbContext
{
    public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Invite> Invites => Set<Invite>();
    public DbSet<AuthIdentity> AuthIdentities => Set<AuthIdentity>();
    public DbSet<FSGSetupBusinessType> FSGSetupBusinessTypes => Set<FSGSetupBusinessType>();
    public DbSet<FSGSetupTimeCardOption> FSGSetupTimeCardOptions => Set<FSGSetupTimeCardOption>();
    public DbSet<FSGSetupAccountingIntegrationType> FSGSetupAccountingIntegrationTypes => Set<FSGSetupAccountingIntegrationType>();
    public DbSet<FSGSetupLanguage> FSGSetupLanguages => Set<FSGSetupLanguage>();
    public DbSet<FSGSetupMasterEntityType> FSGSetupMasterEntityTypes => Set<FSGSetupMasterEntityType>();
    public DbSet<FSGSetupLocationType> FSGSetupLocationTypes => Set<FSGSetupLocationType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("fgs");
        modelBuilder.HasPostgresExtension("citext");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
    }
}
