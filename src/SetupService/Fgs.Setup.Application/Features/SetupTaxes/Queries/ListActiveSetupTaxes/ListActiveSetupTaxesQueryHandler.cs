using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.ListActiveSetupTaxes;

public sealed class ListActiveSetupTaxesQueryHandler(
    IFgsSetupTaxReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveSetupTaxesQuery, ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>> Handle(
        ListActiveSetupTaxesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var tenantScope = tenantContextAccessor.Current;
            if (tenantScope?.IsResolved == true)
            {
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
                    "taxes",
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
                            request.Filters ?? new FgsSetupTaxListFilters(),
                            cancellationToken);
                    },
                    cancellationToken: cancellationToken);

                return ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>.Ok(cached!);
            }

            var listQuery = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                listQuery,
                request.Filters ?? new FgsSetupTaxListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupTaxSummaryDto>>(ex);
        }
    }
}
