using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Queries.ListActiveUniversalMatrixOneTimeFees;

public sealed class ListActiveUniversalMatrixOneTimeFeesQueryHandler(
    IFgsUniversalMatrixOneTimeFeeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveUniversalMatrixOneTimeFeesQuery, ApiResponse<PagedResult<FgsUniversalMatrixOneTimeFeeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalMatrixOneTimeFeeSummaryDto>>> Handle(
        ListActiveUniversalMatrixOneTimeFeesQuery request,
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
            "universalmatrixonetimefee",
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
                    request.Filters ?? new FgsUniversalMatrixOneTimeFeeListFilters(),
                    cancellationToken);
            },
            cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsUniversalMatrixOneTimeFeeSummaryDto>>.Ok(cached!);
    }
}
