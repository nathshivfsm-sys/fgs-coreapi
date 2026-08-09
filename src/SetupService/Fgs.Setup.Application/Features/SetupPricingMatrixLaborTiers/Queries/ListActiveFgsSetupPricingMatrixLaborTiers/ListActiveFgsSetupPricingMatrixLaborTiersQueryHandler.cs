using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.ListActiveFgsSetupPricingMatrixLaborTiers;

public sealed class ListActiveFgsSetupPricingMatrixLaborTiersQueryHandler(IFgsSetupPricingMatrixLaborTierReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<ListActiveFgsSetupPricingMatrixLaborTiersQuery, ApiResponse<PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>>> Handle(ListActiveFgsSetupPricingMatrixLaborTiersQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var segment = CacheKeys.ListActiveSegment(request.Page, request.PageSize, request.SortBy, request.SortDirection.ToString(), request.Search, CacheKeys.Fingerprint(request.Filters));
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixlabortier", segment);
        var result = await cache.GetOrSetAsync(key, () => readRepository.ListAsync(
            new SetupListQuery(request.Page, request.PageSize, request.SortBy, request.SortDirection, request.Search, true),
            request.Filters ?? new FgsSetupPricingMatrixLaborTierListFilters(), cancellationToken), cancellationToken: cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>>.Ok(result!);
    }
}
