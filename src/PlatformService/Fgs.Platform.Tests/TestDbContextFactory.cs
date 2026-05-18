using Fgs.Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Platform.Tests;

internal static class TestDbContextFactory
{
    public static FgsPlatformDbContext Create()
    {
        var options = new DbContextOptionsBuilder<FgsPlatformDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new FgsPlatformDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
