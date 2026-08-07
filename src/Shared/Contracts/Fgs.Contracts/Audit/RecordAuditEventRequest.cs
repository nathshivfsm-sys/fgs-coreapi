namespace Fgs.Contracts.Audit;

/// <summary>
/// S2S request to record a tenant/company-scoped audit event with optional details and attachments.
/// Enum-like fields use the Postgres enum names (e.g. WEB, WORK_ORDER, FIELD_CHANGE).
/// </summary>
public sealed record RecordAuditEventRequest(
    long TenantId,
    long CompanyId,
    string EventCode,
    string EventSource,
    string RecordType,
    long EntityId,
    string Summary,
    DateTime? OccurredOn = null,
    string? EntityNumber = null,
    string? UserName = null,
    IReadOnlyList<RecordAuditEventDetailRequest>? Details = null,
    IReadOnlyList<RecordAuditEventAttachmentRequest>? Attachments = null);

public sealed record RecordAuditEventDetailRequest(
    string EntryType,
    string ItemName,
    string? OldValue = null,
    string? NewValue = null,
    short? Sequence = null);

public sealed record RecordAuditEventAttachmentRequest(
    long DocumentId,
    string? Description = null);
