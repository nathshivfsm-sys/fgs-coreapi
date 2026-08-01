using Fgs.ServiceAgreement.Domain.Entities;
using Fgs.ServiceAgreement.Infrastructure.Database.Configurations;
using Fgs.ServiceAgreement.Infrastructure.Database.Schemas;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.ServiceAgreement.Infrastructure.Database;

public sealed class FgsServiceAgreementDbContext(DbContextOptions<FgsServiceAgreementDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    public DbSet<FgsServiceAgreement> FgsServiceAgreements => Set<FgsServiceAgreement>();

    public DbSet<FgsServiceAgreementCoveredAsset> FgsServiceAgreementCoveredAssets => Set<FgsServiceAgreementCoveredAsset>();

    public DbSet<FgsServiceAgreementVisit> FgsServiceAgreementVisits => Set<FgsServiceAgreementVisit>();

    public DbSet<FgsServiceAgreementVisitAsset> FgsServiceAgreementVisitAssets => Set<FgsServiceAgreementVisitAsset>();

    public DbSet<FgsServiceAgreementBillingSchedule> FgsServiceAgreementBillingSchedules => Set<FgsServiceAgreementBillingSchedule>();

    public DbSet<FgsServiceAgreementNote> FgsServiceAgreementNotes => Set<FgsServiceAgreementNote>();

    public DbSet<FgsServiceAgreementVisitItem> FgsServiceAgreementVisitItems => Set<FgsServiceAgreementVisitItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Svc);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsServiceAgreementDbContext).Assembly);
        FgsServiceAgreementDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
        modelBuilder.ConfigureAuditActorColumns();
    }
}
