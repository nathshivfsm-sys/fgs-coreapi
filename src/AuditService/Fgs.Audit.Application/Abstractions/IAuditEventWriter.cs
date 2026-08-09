using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Contracts.Audit;

namespace Fgs.Audit.Application.Abstractions;

public interface IAuditEventWriter
{
    Task<AuditEventDetailDto> WriteAsync(
        RecordAuditEventRequest request,
        CancellationToken cancellationToken = default);
}
