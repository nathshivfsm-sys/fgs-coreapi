using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Billing.Infrastructure.Database;

public sealed class FgsBillingDbContextDesignFactory : IDesignTimeDbContextFactory<FgsBillingDbContext>
{
    public FgsBillingDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_BILLING_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_BILLING_DB or run dotnet ef with --startup-project Fgs.Billing.API.");

        var options = new DbContextOptionsBuilder<FgsBillingDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsBillingDbContext.MigrationHistorySchema))
            .Options;

        return new FgsBillingDbContext(options);
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.Billing.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsBilling");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "BillingService", "Fgs.Billing.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsBilling");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
