using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.ServiceAgreement.Infrastructure.Database;

public sealed class FgsServiceAgreementDbContextDesignFactory : IDesignTimeDbContextFactory<FgsServiceAgreementDbContext>
{
    public FgsServiceAgreementDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_SVC_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_SVC_DB or run dotnet ef with --startup-project Fgs.ServiceAgreement.API.");

        var options = new DbContextOptionsBuilder<FgsServiceAgreementDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsServiceAgreementDbContext.MigrationHistorySchema))
            .Options;

        return new FgsServiceAgreementDbContext(
            options,
            new Fgs.MultiTenancy.Persistence.DesignTimeTenantContextAccessor());
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.ServiceAgreement.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsServiceAgreement");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "ServiceAgreementService", "Fgs.ServiceAgreement.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsServiceAgreement");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
