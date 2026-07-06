using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.PatchLeadDisqualificationReason;

public sealed class PatchLeadDisqualificationReasonCommandHandler(
    ILeadDisqualificationReasonWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchLeadDisqualificationReasonCommandHandler> logger)
    : IRequestHandler<PatchLeadDisqualificationReasonCommand, ApiResponse<LeadDisqualificationReasonDetailDto>>
{
    public async Task<ApiResponse<LeadDisqualificationReasonDetailDto>> Handle(
        PatchLeadDisqualificationReasonCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd lead disqualification reason {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "leaddisqualificationreason"),
                cancellationToken);
        return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(result);
    }
}
