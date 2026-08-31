using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.TermsConditions;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TermsConditions.Commands.PatchFgsTermsCondition;

public sealed class PatchFgsTermsConditionCommandHandler(
    IFgsTermsConditionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsTermsConditionCommandHandler> logger)
    : IRequestHandler<PatchFgsTermsConditionCommand, ApiResponse<FgsTermsConditionDetailDto>>
{
    public async Task<ApiResponse<FgsTermsConditionDetailDto>> Handle(
        PatchFgsTermsConditionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched terms condition {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "termscondition"),
            cancellationToken);
        return ApiResponse<FgsTermsConditionDetailDto>.Ok(result);
    }
}
