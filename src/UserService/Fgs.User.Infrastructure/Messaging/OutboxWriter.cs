using Fgs.Messaging.Abstractions;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.Messaging.Options;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Messaging;

public sealed class OutboxWriter : IOutboxWriter
{
    private readonly FgsUserDbContext _context;
    private readonly IDateTimeProvider _dateTime;
    private readonly OutboxOptions _options;

    public OutboxWriter(
        FgsUserDbContext context,
        IDateTimeProvider dateTime,
        IOptions<OutboxOptions> options)
    {
        _context = context;
        _dateTime = dateTime;
        _options = options.Value;
    }

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
            MaxRetryCount = _options.MaxRetryCount,
            CreatedOn = _dateTime.UtcNow,
            CreatedBy = createdBy?.ToString()
        };

        await _context.GloOutboxMessages.AddAsync(message, cancellationToken);
    }
}
