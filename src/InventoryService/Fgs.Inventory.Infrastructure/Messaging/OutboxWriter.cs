using Fgs.Foundation.Time;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Domain.Enums;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Inventory.Infrastructure.Messaging;

public sealed class OutboxWriter(
    FgsInventoryDbContext context,
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
        var message = new InventoryOutboxMessage
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

        await context.InventoryOutboxMessages.AddAsync(message, cancellationToken);
    }
}
