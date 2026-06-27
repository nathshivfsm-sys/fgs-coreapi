using Fgs.Notification.Infrastructure.Notifications.Queues;
using FluentAssertions;

namespace Fgs.Notification.Tests.Notifications;

public sealed class IdempotencyStoreTests
{
    [Fact]
    public async Task HasBeenProcessedAsync_ReturnsTrue_WhenMessageAlreadyProcessed()
    {
        await using var context = TestDbContextFactory.Create();
        var store = new IdempotencyStore(context);

        await store.TryMarkProcessedAsync("message-1", "user.UserInvited");

        (await store.HasBeenProcessedAsync("message-1")).Should().BeTrue();
    }

    [Fact]
    public async Task TryMarkProcessedAsync_ReturnsFalse_OnDuplicateMessageId()
    {
        await using var context = TestDbContextFactory.Create();
        var store = new IdempotencyStore(context);

        (await store.TryMarkProcessedAsync("message-1", "user.UserInvited")).Should().BeTrue();
        (await store.TryMarkProcessedAsync("message-1", "user.UserInvited")).Should().BeFalse();
    }
}
