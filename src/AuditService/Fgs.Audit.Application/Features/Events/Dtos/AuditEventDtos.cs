namespace Fgs.Audit.Application.Features.Events.Dtos;

public sealed record AuditEventSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string EventCode,
    string EventSource,
    string RecordType,
    long EntityId,
    string? EntityNumber,
    string? UserName,
    string Summary,
    DateTime OccurredOn,
    DateTime CreatedOn);

public sealed record AuditEventDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string EventCode,
    string EventSource,
    string RecordType,
    long EntityId,
    string? EntityNumber,
    string? UserName,
    string Summary,
    DateTime OccurredOn,
    DateTime CreatedOn,
    IReadOnlyList<AuditEventDetailEntryDto> Details,
    IReadOnlyList<AuditEventAttachmentDto> Attachments);

public sealed record AuditEventDetailEntryDto(
    long Id,
    string EntryType,
    short Sequence,
    string ItemName,
    string? OldValue,
    string? NewValue,
    DateTime CreatedOn);

public sealed record AuditEventAttachmentDto(
    long Id,
    long DocumentId,
    string? Description,
    DateTime CreatedOn);
