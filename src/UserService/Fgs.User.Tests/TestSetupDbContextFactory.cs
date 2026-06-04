using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.User.Tests;

internal static class TestSetupDbContextFactory
{
    public static FgsSetupDbContext Create(ITenantContextAccessor? tenantContextAccessor = null)
    {
        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase($"setup-{Guid.NewGuid()}")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new FgsSetupDbContext(
            options,
            tenantContextAccessor ?? new DesignTimeTenantContextAccessor());
    }

    public static async Task<FgsSetupDbContext> CreateAndInitializeAsync(
        ITenantContextAccessor? tenantContextAccessor = null)
    {
        var context = Create(tenantContextAccessor);
        await context.Database.EnsureCreatedAsync();
        return context;
    }
}
