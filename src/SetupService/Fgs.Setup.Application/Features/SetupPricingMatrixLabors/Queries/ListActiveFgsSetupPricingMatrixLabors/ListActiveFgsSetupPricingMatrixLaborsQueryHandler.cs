using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.ListActiveFgsSetupPricingMatrixLabors;

public sealed class ListActiveFgsSetupPricingMatrixLaborsQueryHandler(IFgsSetupPricingMatrixLaborReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<ListActiveFgsSetupPricingMatrixLaborsQuery, ApiResponse<PagedResult<FgsSetupPricingMatrixLaborSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPricingMatrixLaborSummaryDto>>> Handle(ListActiveFgsSetupPricingMatrixLaborsQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var segment = CacheKeys.ListActiveSegment(request.Page, request.PageSize, request.SortBy, request.SortDirection.ToString(), request.Search, CacheKeys.Fingerprint(request.Filters));
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixlabor", segment);
        var result = await cache.GetOrSetAsync(key, () => readRepository.ListAsync(
            new SetupListQuery(request.Page, request.PageSize, request.SortBy, request.SortDirection, request.Search, true),
            request.Filters ?? new FgsSetupPricingMatrixLaborListFilters(), cancellationToken), cancellationToken: cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPricingMatrixLaborSummaryDto>>.Ok(result!);
    }
}
