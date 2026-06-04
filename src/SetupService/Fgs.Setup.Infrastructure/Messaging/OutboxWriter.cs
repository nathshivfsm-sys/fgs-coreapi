using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Options;
using Fgs.Setup.Application.Abstractions.Time;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Domain.Enums;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Infrastructure.Messaging;

public sealed class OutboxWriter(
    FgsSetupDbContext context,
    IDateTimeProvider dateTime,
    IOptions<OutboxOptions> options) : IOutboxWriter
{
    public async Task EnqueueAsync(
        string eventType,
        string payload,
        Guid correlationId,
        long? tenantId = null,
        long? companyId = null,
        string? aggregateType = null,
        string? aggregateId = null,
        Guid? causationId = null,
        string? exchangeName = null,
        string? routingKey = null,
        string? headers = null,
        long? createdBy = null,
        CancellationToken cancellationToken = default)
    {
        var message = new GloOutboxMessage
        {
            TenantId = tenantId,
            CompanyId = companyId,
            EventType = eventType,
            AggregateType = aggregateType,
            AggregateId = aggregateId,
            CorrelationId = correlationId,
            CausationId = causationId,
            ExchangeName = exchangeName,
            RoutingKey = routingKey,
            Payload = payload,
            Headers = headers,
            Status = OutboxMessageStatus.Pending,
            MaxRetryCount = options.Value.MaxRetryCount,
            CreatedOn = dateTime.UtcNow,
            CreatedBy = createdBy?.ToString()
        };

        await context.GloOutboxMessages.AddAsync(message, cancellationToken);
    }
}
