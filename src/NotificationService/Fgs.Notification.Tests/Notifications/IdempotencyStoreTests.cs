using Fgs.Notification.Infrastructure.Notifications.Queues;
using FluentAssertions;

namespace Fgs.Notification.Tests.Notifications;

public sealed class IdempotencyStoreTests
{
    [Fact]
    public async Task TryMarkProcessedAsync_ReturnsFalse_OnDuplicateMessageId()
    {
        await using var context = TestDbContextFactory.Create();
        var store = new IdempotencyStore(context);

        (await store.TryMarkProcessedAsync("message-1", "user.UserInvited")).Should().BeTrue();
        (await store.TryMarkProcessedAsync("message-1", "user.UserInvited")).Should().BeFalse();
    }
}
