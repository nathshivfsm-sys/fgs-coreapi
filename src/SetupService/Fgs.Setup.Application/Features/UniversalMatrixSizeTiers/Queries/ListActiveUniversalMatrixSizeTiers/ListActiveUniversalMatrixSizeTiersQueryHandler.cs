using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.ListActiveUniversalMatrixSizeTiers;

public sealed class ListActiveUniversalMatrixSizeTiersQueryHandler(
    IFgsUniversalMatrixSizeTierReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveUniversalMatrixSizeTiersQuery, ApiResponse<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>>> Handle(
        ListActiveUniversalMatrixSizeTiersQuery request,
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
            "universalmatrixsizetier",
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
                    request.Filters ?? new FgsUniversalMatrixSizeTierListFilters(),
                    cancellationToken);
            },
            cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>>.Ok(cached!);
    }
}
