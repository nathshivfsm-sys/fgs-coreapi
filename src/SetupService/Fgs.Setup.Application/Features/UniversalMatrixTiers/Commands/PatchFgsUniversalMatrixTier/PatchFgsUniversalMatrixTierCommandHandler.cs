using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.PatchFgsUniversalMatrixTier;

public sealed class PatchFgsUniversalMatrixTierCommandHandler(
    IFgsUniversalMatrixTierWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsUniversalMatrixTierCommandHandler> logger)
    : IRequestHandler<PatchFgsUniversalMatrixTierCommand, ApiResponse<FgsUniversalMatrixTierDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixTierDetailDto>> Handle(
        PatchFgsUniversalMatrixTierCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched universal matrix tier {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixtier"),
            cancellationToken);
        return ApiResponse<FgsUniversalMatrixTierDetailDto>.Ok(result);
    }
}
