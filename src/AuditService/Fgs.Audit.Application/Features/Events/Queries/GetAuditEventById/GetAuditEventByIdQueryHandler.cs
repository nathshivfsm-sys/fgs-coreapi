using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Audit.Application.Features.Events.Queries.GetAuditEventById;

public sealed class GetAuditEventByIdQueryHandler(
    IAuditEventReadRepository readRepository,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetAuditEventByIdQuery, ApiResponse<AuditEventDetailDto>>
{
    public async Task<ApiResponse<AuditEventDetailDto>> Handle(
        GetAuditEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (tenantContextAccessor.Current is not { } scope)
        {
            return ApiResponse<AuditEventDetailDto>.Fail(
                ["Tenant context is required."],
                ApiStatusCodes.BadRequest);
        }

        var result = await readRepository.GetByIdAsync(
            request.Id,
            scope.TenantId,
            scope.CompanyId,
            cancellationToken);
        if (result is null)
        {
            return ApiResponse<AuditEventDetailDto>.Fail(
                [$"Audit event '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<AuditEventDetailDto>.Ok(result);
    }
}
