using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fgs.User.Infrastructure.Database;

/// <summary>
/// Design-time factory for <c>dotnet ef</c> migrations (connection string from <c>FGS_USER_DB</c> or a local default).
/// </summary>
public sealed class FgsUserDbContextDesignFactory : IDesignTimeDbContextFactory<FgsUserDbContext>
{
    public FgsUserDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_USER_DB")
            ?? "Host=localhost;Port=5432;Database=fgs_user;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<FgsUserDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsUserDbContext.FgsSchema))
            .Options;

        return new FgsUserDbContext(options);
    }
}
