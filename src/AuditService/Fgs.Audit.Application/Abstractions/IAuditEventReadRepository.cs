using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Audit.Domain.Enums;

namespace Fgs.Audit.Application.Abstractions;

public interface IAuditEventReadRepository
{
    Task<AuditEventDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditEventSummaryDto>> ListByEntityAsync(
        AuditRecordType recordType,
        long entityId,
        long? tenantId = null,
        long? companyId = null,
        CancellationToken cancellationToken = default);
}
