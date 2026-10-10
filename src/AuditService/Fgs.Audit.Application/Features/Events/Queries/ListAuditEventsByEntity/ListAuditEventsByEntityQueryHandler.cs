using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Audit.Domain.Enums;
using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Audit.Application.Features.Events.Queries.ListAuditEventsByEntity;

public sealed class ListAuditEventsByEntityQueryHandler(
    IAuditEventReadRepository readRepository,
    ITenantContextAccessor tenantContextAccessor)
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

        var scope = ResolveScope(request);
        if (scope.Error is not null)
        {
            return scope.Error;
        }

        var result = await readRepository.ListByEntityAsync(
            recordType,
            request.EntityId,
            scope.TenantId,
            scope.CompanyId,
            cancellationToken);

        return ApiResponse<IReadOnlyList<AuditEventSummaryDto>>.Ok(result);
    }

    private (long TenantId, long CompanyId, ApiResponse<IReadOnlyList<AuditEventSummaryDto>>? Error) ResolveScope(
        ListAuditEventsByEntityQuery request)
    {
        if (tenantContextAccessor.Current is { } current)
        {
            if (request.TenantId is { } requestedTenant && requestedTenant != current.TenantId)
            {
                return (0, 0, Mismatch());
            }

            if (request.CompanyId is { } requestedCompany && requestedCompany != current.CompanyId)
            {
                return (0, 0, Mismatch());
            }

            return (current.TenantId, current.CompanyId, null);
        }

        if (request.TenantId is not long tenantId || request.CompanyId is not long companyId)
        {
            return (0, 0, ApiResponse<IReadOnlyList<AuditEventSummaryDto>>.Fail(
                ["TenantId and CompanyId are required when tenant context is not set."],
                ApiStatusCodes.BadRequest));
        }

        return (tenantId, companyId, null);
    }

    private static ApiResponse<IReadOnlyList<AuditEventSummaryDto>> Mismatch() =>
        ApiResponse<IReadOnlyList<AuditEventSummaryDto>>.Fail(
            ["Audit tenant/company does not match the active tenant scope."],
            ApiStatusCodes.Forbidden);
}
