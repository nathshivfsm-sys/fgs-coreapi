using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Domain.Enums;
using Fgs.Inventory.Infrastructure.Database.Configurations;
using Fgs.Inventory.Infrastructure.Database.Schemas;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.Database;

public sealed class FgsInventoryDbContext : FgsTenantFilteredDbContext
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public FgsInventoryDbContext(
        DbContextOptions<FgsInventoryDbContext> options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options, tenantContextAccessor)
    {
    }

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    public DbSet<InventoryOutboxMessage> InventoryOutboxMessages => Set<InventoryOutboxMessage>();

    public DbSet<FgsInventoryItemType> FgsInventoryItemTypes => Set<FgsInventoryItemType>();

    public DbSet<FgsInventoryCategory> FgsInventoryCategories => Set<FgsInventoryCategory>();

    public DbSet<FgsInventorySubCategory> FgsInventorySubCategories => Set<FgsInventorySubCategory>();

    public DbSet<FgsVendor> FgsVendors => Set<FgsVendor>();

    public DbSet<FgsInventoryLocation> FgsInventoryLocations => Set<FgsInventoryLocation>();

    public DbSet<FgsInventoryItem> FgsInventoryItems => Set<FgsInventoryItem>();

    public DbSet<FgsInventoryItemAlternate> FgsInventoryItemAlternates => Set<FgsInventoryItemAlternate>();

    public DbSet<FgsInventoryItemDependency> FgsInventoryItemDependencies => Set<FgsInventoryItemDependency>();

    public DbSet<FgsInventoryStock> FgsInventoryStocks => Set<FgsInventoryStock>();

    public DbSet<FgsVendorInventoryItem> FgsVendorInventoryItems => Set<FgsVendorInventoryItem>();

    public DbSet<FgsInventoryTransaction> FgsInventoryTransactions => Set<FgsInventoryTransaction>();

    public DbSet<FgsPurchaseOrder> FgsPurchaseOrders => Set<FgsPurchaseOrder>();

    public DbSet<FgsPurchaseOrderDetail> FgsPurchaseOrderDetails => Set<FgsPurchaseOrderDetail>();

    public DbSet<FgsTruckStockTemplate> FgsTruckStockTemplates => Set<FgsTruckStockTemplate>();

    public DbSet<FgsTruckStockTemplateItem> FgsTruckStockTemplateItems => Set<FgsTruckStockTemplateItem>();

    public DbSet<FgsInventorySerial> FgsInventorySerials => Set<FgsInventorySerial>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<FgsInventorySerialStatus>(
            FgsDatabaseSchemas.Inventory,
            "FgsInventorySerialStatus",
            nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());

        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Inventory);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsInventoryDbContext).Assembly);
        FgsInventoryDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
    }
}
