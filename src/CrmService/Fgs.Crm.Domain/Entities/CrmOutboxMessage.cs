using Fgs.Crm.Domain.Enums;
using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

/// <summary>
/// Transactional outbox row for reliable integration event publishing from the CRM schema.
/// </summary>
public class CrmOutboxMessage : GloEntityBase
{
    public long Id { get; set; }

    public long? TenantId { get; set; }

    public long? CompanyId { get; set; }

    public string EventType { get; set; } = null!;

    public string? AggregateType { get; set; }

    public string? AggregateId { get; set; }

    public Guid CorrelationId { get; set; }

    public Guid? CausationId { get; set; }

    public string? ExchangeName { get; set; }

    public string? RoutingKey { get; set; }

    public string Payload { get; set; } = null!;

    public string? Headers { get; set; }

    public OutboxMessageStatus Status { get; set; } = OutboxMessageStatus.Pending;

    public int RetryCount { get; set; }

    public int MaxRetryCount { get; set; } = 10;

    public DateTimeOffset? NextRetryOn { get; set; }

    public DateTimeOffset? ProcessedOn { get; set; }

    public string? LastError { get; set; }
}
