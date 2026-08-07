using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.Events.Queries.ListAuditEventsByEntity;

public sealed record ListAuditEventsByEntityQuery(
    string RecordType,
    long EntityId,
    long? TenantId = null,
    long? CompanyId = null)
    : IRequest<ApiResponse<IReadOnlyList<AuditEventSummaryDto>>>;
