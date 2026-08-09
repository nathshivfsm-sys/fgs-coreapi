using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.ListActiveFgsSetupPricingMatrixOthers;

public sealed class ListActiveFgsSetupPricingMatrixOthersQueryHandler(IFgsSetupPricingMatrixOtherReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<ListActiveFgsSetupPricingMatrixOthersQuery, ApiResponse<PagedResult<FgsSetupPricingMatrixOtherSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPricingMatrixOtherSummaryDto>>> Handle(ListActiveFgsSetupPricingMatrixOthersQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var segment = CacheKeys.ListActiveSegment(request.Page, request.PageSize, request.SortBy, request.SortDirection.ToString(), request.Search, CacheKeys.Fingerprint(request.Filters));
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixother", segment);
        var result = await cache.GetOrSetAsync(key, () => readRepository.ListAsync(
            new SetupListQuery(request.Page, request.PageSize, request.SortBy, request.SortDirection, request.Search, true),
            request.Filters ?? new FgsSetupPricingMatrixOtherListFilters(), cancellationToken), cancellationToken: cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPricingMatrixOtherSummaryDto>>.Ok(result!);
    }
}
