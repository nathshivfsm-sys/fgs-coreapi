using Fgs.User.Application.Abstractions.Messaging;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Database;

namespace Fgs.User.Infrastructure.Messaging;

public sealed class OutboxWriter : IOutboxWriter
{
    private readonly FgsUserDbContext _context;
    private readonly IDateTimeProvider _dateTime;

    public OutboxWriter(FgsUserDbContext context, IDateTimeProvider dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task EnqueueAsync(
        string eventType,
        string payload,
        string idempotencyKey,
        string? correlationId,
        CancellationToken cancellationToken = default)
    {
        var message = new FgsOutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = eventType,
            Payload = payload,
            IdempotencyKey = idempotencyKey,
            CorrelationId = correlationId,
            Status = OutboxMessageStatus.Pending,
            CreatedOn = _dateTime.UtcNow
        };

        await _context.FgsOutboxMessages.AddAsync(message, cancellationToken);
    }
}
