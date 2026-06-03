using Fgs.Messaging.Options;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Enums;
using Fgs.Security.Options;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Tests.Infrastructure;

public sealed class OutboxWriterTests
{
    [Fact]
    public async Task EnqueueAsync_PersistsPendingMessage()
    {
        await using var context = TestDbContextFactory.Create();
        IDateTimeProvider dateTime = new DateTimeProvider();
        var writer = new OutboxWriter(context, dateTime, Options.Create(new OutboxOptions()));
        var correlationId = Guid.NewGuid();

        await writer.EnqueueAsync(
            "TestEvent",
            """{"hello":"world"}""",
            correlationId,
            tenantId: 5001,
            companyId: 200,
            aggregateType: "Tenant",
            aggregateId: "5001");

        await context.SaveChangesAsync();

        var message = await context.GloOutboxMessages.SingleAsync();
        message.EventType.Should().Be("TestEvent");
        message.Status.Should().Be(OutboxMessageStatus.Pending);
        message.CorrelationId.Should().Be(correlationId);
        message.TenantId.Should().Be(5001);
        message.CompanyId.Should().Be(200);
        message.AggregateType.Should().Be("Tenant");
        message.AggregateId.Should().Be("5001");
    }
}
