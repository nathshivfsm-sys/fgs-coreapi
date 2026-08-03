using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.GetFgsSetupPricingMatrixMaterialTierById;

public sealed class GetFgsSetupPricingMatrixMaterialTierByIdQueryHandler(IFgsSetupPricingMatrixMaterialTierReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<GetFgsSetupPricingMatrixMaterialTierByIdQuery, ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>> Handle(GetFgsSetupPricingMatrixMaterialTierByIdQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixmaterialtier", request.Id.ToString());
        var cached = await cache.GetAsync<FgsSetupPricingMatrixMaterialTierDetailDto>(key, cancellationToken);
        if (cached is not null) return ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>.Ok(cached);
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null) return ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>.Fail(["Pricing Matrix Material Tier '" + request.Id + "' was not found."], ApiStatusCodes.NotFound);
        await cache.SetAsync(key, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>.Ok(result);
    }
}
