using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.LookupFgsSetupPricingMatrixMaterialTiers;

public sealed class LookupFgsSetupPricingMatrixMaterialTiersQueryHandler(IFgsSetupPricingMatrixMaterialTierReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<LookupFgsSetupPricingMatrixMaterialTiersQuery, ApiResponse<IReadOnlyList<FgsSetupPricingMatrixMaterialTierLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixMaterialTierLookupDto>>> Handle(LookupFgsSetupPricingMatrixMaterialTiersQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixmaterialtier", $"{CacheKeys.LookupSegment(request.ActiveOnly)}:parent:{request.PricingMatrixId}");
        var result = await cache.GetOrSetAsync(key, () => readRepository.LookupAsync(request.ActiveOnly, request.PricingMatrixId, cancellationToken), cancellationToken: cancellationToken);
        return ApiResponse<IReadOnlyList<FgsSetupPricingMatrixMaterialTierLookupDto>>.Ok(result ?? Array.Empty<FgsSetupPricingMatrixMaterialTierLookupDto>());
    }
}
