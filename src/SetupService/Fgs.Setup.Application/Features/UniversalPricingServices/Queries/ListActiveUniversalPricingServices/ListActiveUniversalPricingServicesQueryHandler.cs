using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Queries.ListActiveUniversalPricingServices;

public sealed class ListActiveUniversalPricingServicesQueryHandler(
    IFgsUniversalPricingServiceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveUniversalPricingServicesQuery, ApiResponse<PagedResult<FgsUniversalPricingServiceSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalPricingServiceSummaryDto>>> Handle(
        ListActiveUniversalPricingServicesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var segment = CacheKeys.ListActiveSegment(
            request.Page,
            request.PageSize,
            request.SortBy,
            request.SortDirection.ToString(),
            request.Search,
            CacheKeys.Fingerprint(request.Filters));

        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalpricingservice",
            segment);

        var cached = await cache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                var query = new SetupListQuery(
                    request.Page,
                    request.PageSize,
                    request.SortBy,
                    request.SortDirection,
                    request.Search,
                    IsActive: true);

                return await readRepository.ListAsync(
                    query,
                    request.Filters ?? new FgsUniversalPricingServiceListFilters(),
                    cancellationToken);
            },
            cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsUniversalPricingServiceSummaryDto>>.Ok(cached!);
    }
}
