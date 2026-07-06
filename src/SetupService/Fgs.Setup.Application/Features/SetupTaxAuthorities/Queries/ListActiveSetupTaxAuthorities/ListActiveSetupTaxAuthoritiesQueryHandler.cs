using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.ListActiveSetupTaxAuthorities;

public sealed class ListActiveSetupTaxAuthoritiesQueryHandler(
    IFgsSetupTaxAuthorityReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveSetupTaxAuthoritiesQuery, ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>> Handle(
        ListActiveSetupTaxAuthoritiesQuery request,
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
        "taxauthority",
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
                request.Filters ?? new FgsSetupTaxAuthorityListFilters(),
                cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>.Ok(cached!);
    }
}
