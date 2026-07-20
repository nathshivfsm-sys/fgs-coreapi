using Fgs.Credentials;
using Fgs.MultiTenancy.Persistence;
using Fgs.Notification.Domain.Enums;
using Fgs.Notification.Infrastructure.Database.Schemas;
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

        var nullTranslator = new Npgsql.NameTranslation.NpgsqlNullNameTranslator();
        var optionsBuilder = new DbContextOptionsBuilder<FgsNotificationDbContext>();
        optionsBuilder.UseNpgsql(
            ConnectionStringResolver.ResolveRequired(
                configuration,
                ConnectionStringNames.FgsNotification,
                "FGS_NOTIFICATION_DB"),
            npgsql =>
            {
                npgsql.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    FgsNotificationDbContext.MigrationHistorySchema);
                npgsql.MapEnum<NotificationStatus>(
                    "notification_status", FgsDatabaseSchemas.Notification, nameTranslator: nullTranslator);
                npgsql.MapEnum<NotificationSourceApplication>(
                    "source_application", FgsDatabaseSchemas.Notification, nameTranslator: nullTranslator);
            });

        return new FgsNotificationDbContext(optionsBuilder.Options, new DesignTimeTenantContextAccessor());
    }
}
