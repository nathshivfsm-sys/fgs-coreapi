using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Abstractions.Time;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Common.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.Database.Read;
using Fgs.Inventory.Infrastructure.InventoryLocations;
using Fgs.Inventory.Infrastructure.TruckStockTemplateItems;
using Fgs.Inventory.Infrastructure.TruckStockTemplates;
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
        services.AddScoped<IFgsTruckStockTemplateItemReadRepository, FgsTruckStockTemplateItemReadRepository>();
        services.AddScoped<IFgsTruckStockTemplateItemWriteService, FgsTruckStockTemplateItemWriteService>();

        return services;
    }
}
