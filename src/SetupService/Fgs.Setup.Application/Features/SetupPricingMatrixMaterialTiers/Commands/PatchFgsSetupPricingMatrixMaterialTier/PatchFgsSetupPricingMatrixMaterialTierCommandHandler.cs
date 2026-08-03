using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Commands.PatchFgsSetupPricingMatrixMaterialTier;

public sealed class PatchFgsSetupPricingMatrixMaterialTierCommandHandler(
    IFgsSetupPricingMatrixMaterialTierWriteService writeService, ICacheService cache, ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSetupPricingMatrixMaterialTierCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupPricingMatrixMaterialTierCommand, ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>> Handle(PatchFgsSetupPricingMatrixMaterialTierCommand request, CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched Pricing Matrix Material Tier {Id}", result.Id);
        var scope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(CacheKeys.EntityPrefix(scope.TenantId, scope.CompanyId, "pricingmatrixmaterialtier"), cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>.Ok(result);
    }
}
