using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.User.Tests;

internal static class TestDbContextFactory
{
    public static FgsUserDbContext Create(ITenantContextAccessor? tenantContextAccessor = null)
    {
        var options = new DbContextOptionsBuilder<FgsUserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new FgsUserDbContext(
            options,
            tenantContextAccessor ?? new DesignTimeTenantContextAccessor());
    }

    public static async Task<FgsUserDbContext> CreateAndInitializeAsync(
        ITenantContextAccessor? tenantContextAccessor = null)
    {
        var context = Create(tenantContextAccessor);
        await context.Database.EnsureCreatedAsync();
        return context;
    }
}

internal sealed class TestTenantContextAccessor : ITenantContextAccessor
{
    public ITenantContext? Current { get; set; }
}
