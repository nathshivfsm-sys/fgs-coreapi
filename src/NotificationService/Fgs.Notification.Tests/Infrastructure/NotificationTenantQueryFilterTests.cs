using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.Notification.Tests.Infrastructure;

public sealed class NotificationTenantQueryFilterTests
{
    [Fact]
    public async Task NotificationHistory_WhenUnresolved_ReturnsAllRows()
    {
        var context = await CreateContextAsync();
        context.NotificationHistory.AddRange(
            CreateHistory(1, "TENANT_A"),
            CreateHistory(2, "TENANT_B"));
        await context.SaveChangesAsync();

        var history = await context.NotificationHistory.ToListAsync();

        history.Should().HaveCount(2);
    }

    [Fact]
    public async Task NotificationHistory_WhenResolved_FiltersToCurrentTenant()
    {
        var accessor = new NotificationTestTenantContextAccessor
        {
            Current = new TenantContext
            {
                TenantId = 1,
                CompanyId = 1,
                IsResolved = true
            }
        };

        var context = await CreateContextAsync(accessor);
        context.NotificationHistory.AddRange(
            CreateHistory(1, "MATCH"),
            CreateHistory(2, "OTHER"));
        await context.SaveChangesAsync();

        var history = await context.NotificationHistory.ToListAsync();

        history.Should().ContainSingle();
        history[0].TemplateName.Should().Be("MATCH");
    }

    private static async Task<FgsNotificationDbContext> CreateContextAsync(
        ITenantContextAccessor? accessor = null)
    {
        var options = new DbContextOptionsBuilder<FgsNotificationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsNotificationDbContext(
            options,
            accessor ?? new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private static FgsNotificationHistory CreateHistory(long tenantId, string templateName) =>
        new()
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Channel = NotificationChannel.Email,
            TemplateName = templateName,
            Status = NotificationDeliveryStatus.Pending,
            CreatedOn = DateTimeOffset.UtcNow
        };

    private sealed class NotificationTestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
