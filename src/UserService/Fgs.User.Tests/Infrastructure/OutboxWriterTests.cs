using Fgs.Messaging.Options;
using Fgs.Setup.Application.Abstractions.Time;
using Fgs.Setup.Domain.Enums;
using Fgs.Setup.Infrastructure.Common.Time;
using SetupDateTimeProvider = Fgs.Setup.Infrastructure.Common.Time.DateTimeProvider;
using Fgs.Setup.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fgs.User.Tests.Infrastructure;

public sealed class OutboxWriterTests
{
    [Fact]
    public async Task EnqueueAsync_PersistsPendingMessage()
    {
        await using var context = await TestSetupDbContextFactory.CreateAndInitializeAsync();
        IDateTimeProvider dateTime = new SetupDateTimeProvider();
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
