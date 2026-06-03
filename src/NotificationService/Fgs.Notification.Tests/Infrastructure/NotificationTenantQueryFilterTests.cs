using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.Notification.Tests.Infrastructure;

public sealed class NotificationTenantQueryFilterTests
{
    [Fact]
    public async Task CommunicationTemplates_WhenUnresolved_ReturnsGlobalAndTenantRows()
    {
        var context = await CreateContextAsync();
        context.CommunicationTemplates.AddRange(
            CreateTemplate(null, null, "GLOBAL"),
            CreateTemplate(1, 1, "TENANT"));
        await context.SaveChangesAsync();

        var templates = await context.CommunicationTemplates.ToListAsync();

        templates.Should().HaveCount(2);
    }

    [Fact]
    public async Task CommunicationTemplates_WhenResolved_FiltersTenantRowsAndKeepsGlobal()
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
        context.CommunicationTemplates.AddRange(
            CreateTemplate(null, null, "GLOBAL"),
            CreateTemplate(1, 1, "TENANT_MATCH"),
            CreateTemplate(2, 1, "OTHER_TENANT"));
        await context.SaveChangesAsync();

        var templates = await context.CommunicationTemplates.ToListAsync();

        templates.Should().HaveCount(2);
        templates.Should().Contain(t => t.Code == "GLOBAL");
        templates.Should().Contain(t => t.Code == "TENANT_MATCH");
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

    private static FgsSetupCommunicationTemplate CreateTemplate(
        long? tenantId,
        long? companyId,
        string code) =>
        new()
        {
            TenantId = tenantId,
            CompanyId = companyId,
            TemplateType = "EMAIL",
            Code = code,
            Name = code,
            Body = "body",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };

    private sealed class NotificationTestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
