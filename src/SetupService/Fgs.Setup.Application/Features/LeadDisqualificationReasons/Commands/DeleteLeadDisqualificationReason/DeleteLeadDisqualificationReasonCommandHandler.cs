using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.DeleteLeadDisqualificationReason;

public sealed class DeleteLeadDisqualificationReasonCommandHandler(
    ILeadDisqualificationReasonWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteLeadDisqualificationReasonCommandHandler> logger)
    : IRequestHandler<DeleteLeadDisqualificationReasonCommand, ApiResponse<LeadDisqualificationReasonDetailDto>>
{
    public async Task<ApiResponse<LeadDisqualificationReasonDetailDto>> Handle(
        DeleteLeadDisqualificationReasonCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted lead disqualification reason {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "leaddisqualificationreason"),
                cancellationToken);
        return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(result);
    }
}
