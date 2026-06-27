using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Database.Configurations;
using Fgs.Asset.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Asset.Infrastructure.Database;

public sealed class FgsAssetDbContext(DbContextOptions<FgsAssetDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();
    public DbSet<FgsServiceLocationCache> FgsServiceLocationCaches => Set<FgsServiceLocationCache>();
    public DbSet<FgsAssetStatus> FgsAssetStatuses => Set<FgsAssetStatus>();
    public DbSet<FgsAssetType> FgsAssetTypes => Set<FgsAssetType>();
    public DbSet<FgsAssetManufacturer> FgsAssetManufacturers => Set<FgsAssetManufacturer>();
    public DbSet<FgsAssetModel> FgsAssetModels => Set<FgsAssetModel>();
    public DbSet<Domain.Entities.FgsAsset> FgsAssets => Set<Domain.Entities.FgsAsset>();
    public DbSet<FgsAssetWarranty> FgsAssetWarranties => Set<FgsAssetWarranty>();
    public DbSet<FgsAssetAttribute> FgsAssetAttributes => Set<FgsAssetAttribute>();
    public DbSet<FgsAssetAttributeOption> FgsAssetAttributeOptions => Set<FgsAssetAttributeOption>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Asset);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsAssetDbContext).Assembly);
        FgsAssetDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
        ConfigureAuditActorColumns(modelBuilder);
    }

    private static void ConfigureAuditActorColumns(ModelBuilder modelBuilder)
    {
        const int maxLength = 200;
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
