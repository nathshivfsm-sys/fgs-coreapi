using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Commands.CreateFgsSetupPricingMatrixMaterialTier;

public sealed class CreateFgsSetupPricingMatrixMaterialTierCommandHandler(
    IFgsSetupPricingMatrixMaterialTierWriteService writeService, ICacheService cache, ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupPricingMatrixMaterialTierCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupPricingMatrixMaterialTierCommand, ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>> Handle(CreateFgsSetupPricingMatrixMaterialTierCommand request, CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created Pricing Matrix Material Tier {Id}", result.Id);
        var scope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(CacheKeys.EntityPrefix(scope.TenantId, scope.CompanyId, "pricingmatrixmaterialtier"), cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
