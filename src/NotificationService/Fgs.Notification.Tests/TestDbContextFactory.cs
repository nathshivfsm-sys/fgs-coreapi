using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Notification.Tests;

internal static class TestDbContextFactory
{
    public static FgsNotificationDbContext Create(ITenantContextAccessor? tenantContextAccessor = null)
    {
        var options = new DbContextOptionsBuilder<FgsNotificationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new FgsNotificationDbContext(
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
