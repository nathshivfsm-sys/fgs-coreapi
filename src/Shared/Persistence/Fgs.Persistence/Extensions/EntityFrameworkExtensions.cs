using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fgs.Persistence.Extensions;

public static class EntityFrameworkExtensions
{
    public static DbContextOptionsBuilder UseFgsNpgsql(
        this DbContextOptionsBuilder options,
        string connectionString,
        string migrationsHistoryTable,
        string? migrationsHistorySchema = null)
    {
        options.UseNpgsql(connectionString, npgsql =>
        {
            if (migrationsHistorySchema is not null)
            {
                npgsql.MigrationsHistoryTable(migrationsHistoryTable, migrationsHistorySchema);
            }
            else
            {
                npgsql.MigrationsHistoryTable(migrationsHistoryTable);
            }

            npgsql.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null);
        });

        return options;
    }

    public static DbContextOptionsBuilder UseFgsNpgsql(
        this DbContextOptionsBuilder options,
        IConfiguration configuration,
        string connectionStringName,
        string migrationsHistoryTable,
        string? migrationsHistorySchema = null)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)
            ?? throw new InvalidOperationException($"ConnectionStrings:{connectionStringName} is required.");

        return options.UseFgsNpgsql(connectionString, migrationsHistoryTable, migrationsHistorySchema);
    }
}
