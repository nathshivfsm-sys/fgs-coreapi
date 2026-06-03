using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Platform.Infrastructure.Database;

public sealed class FgsPlatformDbContextDesignFactory : IDesignTimeDbContextFactory<FgsPlatformDbContext>
{
    public FgsPlatformDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Fgs.Platform.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<FgsPlatformDbContext>();
        optionsBuilder.UseNpgsql(
            FgsPlatformConnectionString.ResolveRequired(configuration),
            npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsPlatformDbContext.FgsSchema));

        return new FgsPlatformDbContext(optionsBuilder.Options, new DesignTimeTenantContextAccessor());
    }
}
