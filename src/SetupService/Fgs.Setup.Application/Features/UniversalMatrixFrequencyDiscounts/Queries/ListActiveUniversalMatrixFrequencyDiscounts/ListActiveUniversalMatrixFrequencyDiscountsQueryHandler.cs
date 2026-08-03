using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.ListActiveUniversalMatrixFrequencyDiscounts;

public sealed class ListActiveUniversalMatrixFrequencyDiscountsQueryHandler(
    IFgsUniversalMatrixFrequencyDiscountReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveUniversalMatrixFrequencyDiscountsQuery, ApiResponse<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>>> Handle(
        ListActiveUniversalMatrixFrequencyDiscountsQuery request,
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
            "universalmatrixfrequencydiscount",
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
                    request.Filters ?? new FgsUniversalMatrixFrequencyDiscountListFilters(),
                    cancellationToken);
            },
            cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>>.Ok(cached!);
    }
}
