using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Enums;
using Fgs.Notification.Infrastructure.Database;
using Fgs.Notification.Infrastructure.Notifications.History;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.Notification.Tests.Infrastructure;

public sealed class NotificationTenantQueryFilterTests
{
    [Fact]
    public async Task EmailHistory_WhenUnresolved_ReturnsAllRows()
    {
        var context = await CreateContextAsync();
        context.FgsEmailHistories.AddRange(
            CreateEmailHistory(1, "TENANT_A"),
            CreateEmailHistory(2, "TENANT_B"));
        await context.SaveChangesAsync();

        var history = await context.FgsEmailHistories.ToListAsync();

        history.Should().HaveCount(2);
    }

    [Fact]
    public async Task EmailHistory_WhenResolved_FiltersToCurrentTenant()
    {
        var accessor = new NotificationTestTenantContextAccessor
        {
            Current = new TenantContext
            {
                TenantId = 1,
                CompanyId = 1
            }
        };

        var context = await CreateContextAsync(accessor);
        context.FgsEmailHistories.AddRange(
            CreateEmailHistory(1, "MATCH"),
            CreateEmailHistory(2, "OTHER"));
        await context.SaveChangesAsync();

        var history = await context.FgsEmailHistories.ToListAsync();

        history.Should().ContainSingle();
        history[0].Subject.Should().Be("MATCH");
    }

    [Fact]
    public async Task UpdateEmailStatus_WhenDifferentTenantContext_LeavesRowUnchanged()
    {
        var accessor = new NotificationTestTenantContextAccessor
        {
            Current = new TenantContext
            {
                TenantId = 1,
                CompanyId = 1
            }
        };

        var context = await CreateContextAsync(accessor);
        var history = CreateEmailHistory(1, "MATCH");
        context.FgsEmailHistories.Add(history);
        await context.SaveChangesAsync();

        accessor.Current = new TenantContext
        {
            TenantId = 2,
            CompanyId = 9
        };

        var repository = new NotificationHistoryRepository(context);
        var sentOn = DateTimeOffset.UtcNow;
        await repository.UpdateEmailStatusAsync(
            history.Id,
            NotificationStatus.Sent,
            "provider-1",
            "Smtp",
            null,
            sentOn,
            null);

        var updated = await context.FgsEmailHistories
            .IgnoreQueryFilters()
            .SingleAsync(h => h.Id == history.Id);

        updated.Status.Should().Be(NotificationStatus.Queued);
        updated.ProviderMessageId.Should().BeNull();
        updated.SentOn.Should().BeNull();
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

    private static FgsEmailHistory CreateEmailHistory(long tenantId, string subject) =>
        new()
        {
            TenantId = tenantId,
            CompanyId = 1,
            RecordType = "USER",
            RecordId = 1,
            Status = NotificationStatus.Queued,
            SourceApplication = NotificationSourceApplication.Api,
            Subject = subject,
            FromEmailAddress = "noreply@fgs.local",
            ToEmailAddresses = "[\"a@b.com\"]",
            Body = "body",
            CreatedOn = DateTimeOffset.UtcNow
        };

    private sealed class NotificationTestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
