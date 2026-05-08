using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UserService.Infrastructure.Persistence;

public sealed class UserServiceDbContextFactory : IDesignTimeDbContextFactory<UserServiceDbContext>
{
    public UserServiceDbContext CreateDbContext(string[] args)
    {
        var apiProjectPath = ResolveApiProjectDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("UserServiceDb")
            ?? "Host=127.0.0.1;Port=5432;Database=user_service_migrations;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<UserServiceDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsql =>
            npgsql.MigrationsHistoryTable("__ef_migrations_history", "fgs"));

        return new UserServiceDbContext(optionsBuilder.Options);
    }

    private static string ResolveApiProjectDirectory()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "UserService.API");
            if (Directory.Exists(candidate) && File.Exists(Path.Combine(candidate, "UserService.API.csproj")))
                return candidate;

            dir = dir.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}
