using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.ListActiveResolutionCodes;

public sealed class ListActiveResolutionCodesQueryHandler(
    IResolutionCodeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveResolutionCodesQuery, ApiResponse<PagedResult<ResolutionCodeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<ResolutionCodeSummaryDto>>> Handle(
        ListActiveResolutionCodesQuery request,
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
        "resolutioncodes",
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
                request.Filters ?? new ResolutionCodeListFilters(),
                cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<ResolutionCodeSummaryDto>>.Ok(cached!);
    }
}
