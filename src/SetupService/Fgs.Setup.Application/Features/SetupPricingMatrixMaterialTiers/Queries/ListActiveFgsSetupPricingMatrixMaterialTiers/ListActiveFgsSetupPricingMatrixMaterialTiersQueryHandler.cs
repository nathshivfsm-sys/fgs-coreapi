using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.ListActiveFgsSetupPricingMatrixMaterialTiers;

public sealed class ListActiveFgsSetupPricingMatrixMaterialTiersQueryHandler(IFgsSetupPricingMatrixMaterialTierReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<ListActiveFgsSetupPricingMatrixMaterialTiersQuery, ApiResponse<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>>> Handle(ListActiveFgsSetupPricingMatrixMaterialTiersQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var segment = CacheKeys.ListActiveSegment(request.Page, request.PageSize, request.SortBy, request.SortDirection.ToString(), request.Search, CacheKeys.Fingerprint(request.Filters));
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixmaterialtier", segment);
        var result = await cache.GetOrSetAsync(key, () => readRepository.ListAsync(
            new SetupListQuery(request.Page, request.PageSize, request.SortBy, request.SortDirection, request.Search, true),
            request.Filters ?? new FgsSetupPricingMatrixMaterialTierListFilters(), cancellationToken), cancellationToken: cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>>.Ok(result!);
    }
}
