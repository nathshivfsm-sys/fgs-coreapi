using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Abstractions.InventoryTransactions;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Abstractions.Time;
using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Common.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.Database.Read;
using Fgs.Inventory.Infrastructure.InventoryCategories;
using Fgs.Inventory.Infrastructure.InventoryItems;
using Fgs.Inventory.Infrastructure.InventoryItemTypes;
using Fgs.Inventory.Infrastructure.InventoryLocations;
using Fgs.Inventory.Infrastructure.InventoryStocks;
using Fgs.Inventory.Infrastructure.InventorySubCategories;
using Fgs.Inventory.Infrastructure.InventoryTransactions;
using Fgs.Inventory.Infrastructure.PurchaseOrders;
using Fgs.Inventory.Infrastructure.TruckStockTemplates;
using Fgs.Inventory.Infrastructure.VendorInventoryItems;
using Fgs.Inventory.Infrastructure.Vendors;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsInventoryInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-inventory-service", "DATABASE");

        services.AddDbContext<FgsInventoryDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsInventory,
                FgsInventoryConnectionString.EnvironmentVariable,
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsInventoryDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsInventoryDbContext>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IInventoryReadConnectionFactory, FgsInventoryReadConnectionFactory>();
        services.AddScoped<InventoryEntityAuditHelper>();
        services.AddScoped<IFgsInventoryLocationReadRepository, FgsInventoryLocationReadRepository>();
        services.AddScoped<IFgsInventoryLocationWriteService, FgsInventoryLocationWriteService>();
        services.AddScoped<IFgsVendorReadRepository, FgsVendorReadRepository>();
        services.AddScoped<IFgsVendorWriteService, FgsVendorWriteService>();
        services.AddScoped<IFgsTruckStockTemplateReadRepository, FgsTruckStockTemplateReadRepository>();
        services.AddScoped<IFgsTruckStockTemplateWriteService, FgsTruckStockTemplateWriteService>();
        services.AddScoped<IFgsPurchaseOrderReadRepository, FgsPurchaseOrderReadRepository>();
        services.AddScoped<IFgsPurchaseOrderWriteService, FgsPurchaseOrderWriteService>();
        services.AddScoped<IFgsInventoryItemReadRepository, FgsInventoryItemReadRepository>();
        services.AddScoped<IFgsInventoryItemWriteService, FgsInventoryItemWriteService>();
        services.AddScoped<IFgsInventoryItemTypeReadRepository, FgsInventoryItemTypeReadRepository>();
        services.AddScoped<IFgsInventoryItemTypeWriteService, FgsInventoryItemTypeWriteService>();
        services.AddScoped<IFgsInventoryCategoryReadRepository, FgsInventoryCategoryReadRepository>();
        services.AddScoped<IFgsInventoryCategoryWriteService, FgsInventoryCategoryWriteService>();
        services.AddScoped<IFgsInventorySubCategoryReadRepository, FgsInventorySubCategoryReadRepository>();
        services.AddScoped<IFgsInventorySubCategoryWriteService, FgsInventorySubCategoryWriteService>();
        services.AddScoped<IFgsVendorInventoryItemReadRepository, FgsVendorInventoryItemReadRepository>();
        services.AddScoped<IFgsVendorInventoryItemWriteService, FgsVendorInventoryItemWriteService>();
        services.AddScoped<IFgsInventoryStockReadRepository, FgsInventoryStockReadRepository>();
        services.AddScoped<IFgsInventoryStockWriteService, FgsInventoryStockWriteService>();
        services.AddScoped<IFgsInventoryTransactionReadRepository, FgsInventoryTransactionReadRepository>();
        services.AddScoped<IFgsInventoryTransactionWriteService, FgsInventoryTransactionWriteService>();

        return services;
    }
}
