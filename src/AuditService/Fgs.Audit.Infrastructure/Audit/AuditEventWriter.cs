using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Audit.Domain.Entities;
using Fgs.Audit.Domain.Enums;
using Fgs.Audit.Infrastructure.Database;
using Fgs.Contracts.Audit;

namespace Fgs.Audit.Infrastructure.Audit;

public sealed class AuditEventWriter(FgsAuditDbContext context) : IAuditEventWriter
{
    public async Task<AuditEventDetailDto> WriteAsync(
        RecordAuditEventRequest request,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var eventSource = Enum.Parse<AuditEventSource>(request.EventSource, ignoreCase: true);
        var recordType = Enum.Parse<AuditRecordType>(request.RecordType, ignoreCase: true);

        var entity = new FgsEvent
        {
            TenantId = request.TenantId,
            CompanyId = request.CompanyId,
            EventCode = request.EventCode.Trim(),
            EventSource = eventSource,
            RecordType = recordType,
            EntityId = request.EntityId,
            EntityNumber = string.IsNullOrWhiteSpace(request.EntityNumber)
                ? null
                : request.EntityNumber.Trim(),
            UserName = string.IsNullOrWhiteSpace(request.UserName)
                ? null
                : request.UserName.Trim(),
            Summary = request.Summary.Trim(),
            OccurredOn = request.OccurredOn ?? now,
            CreatedOn = now
        };

        if (request.Details is { Count: > 0 })
        {
            short sequence = 1;
            foreach (var detail in request.Details)
            {
                entity.Details.Add(new FgsEventDetail
                {
                    EntryType = Enum.Parse<AuditEventDetailType>(detail.EntryType, ignoreCase: true),
                    Sequence = detail.Sequence ?? sequence,
                    ItemName = detail.ItemName.Trim(),
                    OldValue = detail.OldValue,
                    NewValue = detail.NewValue,
                    CreatedOn = now
                });
                sequence++;
            }
        }

        if (request.Attachments is { Count: > 0 })
        {
            foreach (var attachment in request.Attachments)
            {
                entity.Attachments.Add(new FgsEventAttachment
                {
                    DocumentId = attachment.DocumentId,
                    Description = string.IsNullOrWhiteSpace(attachment.Description)
                        ? null
                        : attachment.Description.Trim(),
                    CreatedOn = now
                });
            }
        }

        await context.FgsEvents.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return AuditEventMapper.ToDetailDto(entity);
    }
}
