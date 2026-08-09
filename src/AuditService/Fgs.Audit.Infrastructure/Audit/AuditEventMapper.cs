using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Audit.Domain.Entities;

namespace Fgs.Audit.Infrastructure.Audit;

internal static class AuditEventMapper
{
    public static AuditEventSummaryDto ToSummaryDto(FgsEvent entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.EventCode,
            entity.EventSource.ToString(),
            entity.RecordType.ToString(),
            entity.EntityId,
            entity.EntityNumber,
            entity.UserName,
            entity.Summary,
            entity.OccurredOn,
            entity.CreatedOn);

    public static AuditEventDetailDto ToDetailDto(FgsEvent entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.EventCode,
            entity.EventSource.ToString(),
            entity.RecordType.ToString(),
            entity.EntityId,
            entity.EntityNumber,
            entity.UserName,
            entity.Summary,
            entity.OccurredOn,
            entity.CreatedOn,
            entity.Details
                .OrderBy(d => d.Sequence)
                .ThenBy(d => d.Id)
                .Select(d => new AuditEventDetailEntryDto(
                    d.Id,
                    d.EntryType.ToString(),
                    d.Sequence,
                    d.ItemName,
                    d.OldValue,
                    d.NewValue,
                    d.CreatedOn))
                .ToList(),
            entity.Attachments
                .OrderBy(a => a.Id)
                .Select(a => new AuditEventAttachmentDto(
                    a.Id,
                    a.DocumentId,
                    a.Description,
                    a.CreatedOn))
                .ToList());
}
