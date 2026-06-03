using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Notification.Infrastructure.Database;

public sealed class FgsNotificationDbContextDesignFactory : IDesignTimeDbContextFactory<FgsNotificationDbContext>
{
    public FgsNotificationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Fgs.Notification.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<FgsNotificationDbContext>();
        optionsBuilder.UseNpgsql(
            FgsNotificationConnectionString.ResolveRequired(configuration),
            npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsNotificationDbContext.FgsSchema));

        return new FgsNotificationDbContext(optionsBuilder.Options, new DesignTimeTenantContextAccessor());
    }
}
