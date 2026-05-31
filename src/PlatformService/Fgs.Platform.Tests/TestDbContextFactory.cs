using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Platform.Tests;

internal static class TestDbContextFactory
{
    public static FgsPlatformDbContext Create(ITenantContextAccessor? tenantContextAccessor = null)
    {
        var options = new DbContextOptionsBuilder<FgsPlatformDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new FgsPlatformDbContext(
            options,
            tenantContextAccessor ?? new DesignTimeTenantContextAccessor());
        context.Database.EnsureCreated();
        return context;
    }
}

internal sealed class TestTenantContextAccessor : ITenantContextAccessor
{
    public ITenantContext? Current { get; set; }
}
