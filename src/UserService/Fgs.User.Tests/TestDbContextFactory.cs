using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.User.Tests;

internal static class TestDbContextFactory
{
    public static FgsUserDbContext Create()
    {
        var options = new DbContextOptionsBuilder<FgsUserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new FgsUserDbContext(options);
    }

    public static async Task<FgsUserDbContext> CreateAndInitializeAsync()
    {
        var context = Create();
        await context.Database.EnsureCreatedAsync();
        return context;
    }
}
