using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Audit.Domain.Enums;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.Events.Queries.ListAuditEventsByEntity;

public sealed class ListAuditEventsByEntityQueryHandler(IAuditEventReadRepository readRepository)
    : IRequestHandler<ListAuditEventsByEntityQuery, ApiResponse<IReadOnlyList<AuditEventSummaryDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<AuditEventSummaryDto>>> Handle(
        ListAuditEventsByEntityQuery request,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<AuditRecordType>(request.RecordType, ignoreCase: true, out var recordType))
        {
            return ApiResponse<IReadOnlyList<AuditEventSummaryDto>>.Fail(
                [$"RecordType '{request.RecordType}' is invalid."],
                ApiStatusCodes.BadRequest);
        }

        var result = await readRepository.ListByEntityAsync(
            recordType,
            request.EntityId,
            request.TenantId,
            request.CompanyId,
            cancellationToken);

        return ApiResponse<IReadOnlyList<AuditEventSummaryDto>>.Ok(result);
    }
}
