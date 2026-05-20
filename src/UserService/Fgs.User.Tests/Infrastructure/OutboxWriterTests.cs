using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Tests.Infrastructure;

public sealed class OutboxWriterTests
{
    [Fact]
    public async Task EnqueueAsync_PersistsPendingMessage()
    {
        await using var context = TestDbContextFactory.Create();
        IDateTimeProvider dateTime = new DateTimeProvider();
        var writer = new OutboxWriter(context, dateTime);

        await writer.EnqueueAsync(
            "TestEvent",
            """{"hello":"world"}""",
            "idempotency-1",
            "corr-1");

        await context.SaveChangesAsync();

        var message = await context.FgsOutboxMessages.SingleAsync();
        message.EventType.Should().Be("TestEvent");
        message.Status.Should().Be(OutboxMessageStatus.Pending);
        message.IdempotencyKey.Should().Be("idempotency-1");
    }
}
