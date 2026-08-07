using Fgs.Inventory.Domain.Enums;
using Fgs.Inventory.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Inventory.Infrastructure.Database;

public sealed class FgsInventoryDbContextDesignFactory : IDesignTimeDbContextFactory<FgsInventoryDbContext>
{
    public FgsInventoryDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_INVENTORY_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_INVENTORY_DB or run dotnet ef with --startup-project Fgs.Inventory.API.");

        var options = new DbContextOptionsBuilder<FgsInventoryDbContext>()
            .UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsInventoryDbContext.MigrationHistorySchema);
                npgsql.MapEnum<FgsInventorySerialStatus>(
                    "FgsInventorySerialStatus",
                    FgsDatabaseSchemas.Inventory,
                    nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
            })
            .Options;

        return new FgsInventoryDbContext(options, new Fgs.MultiTenancy.Persistence.DesignTimeTenantContextAccessor());
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.Inventory.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsInventory");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "InventoryService", "Fgs.Inventory.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsInventory");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
