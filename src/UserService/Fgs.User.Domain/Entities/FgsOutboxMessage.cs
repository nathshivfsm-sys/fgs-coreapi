using Fgs.User.Domain.Enums;

namespace Fgs.User.Domain.Entities;

/// <summary>
/// Transactional outbox row for reliable integration event publishing.
/// </summary>
public class FgsOutboxMessage
{
    public Guid Id { get; set; }

    public string EventType { get; set; } = null!;

    public string Payload { get; set; } = null!;

    public string IdempotencyKey { get; set; } = null!;

    public string? CorrelationId { get; set; }

    public OutboxMessageStatus Status { get; set; } = OutboxMessageStatus.Pending;

    public int RetryCount { get; set; }

    public string? LastError { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ProcessedOn { get; set; }

    public bool IsDeleted { get; set; }
}
