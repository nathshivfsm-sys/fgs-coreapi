using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.ListActiveSetupLaborRateTypes;

public sealed class ListActiveSetupLaborRateTypesQueryHandler(
    IFgsSetupLaborRateTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveSetupLaborRateTypesQuery, ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>> Handle(
        ListActiveSetupLaborRateTypesQuery request,
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
                    "laborratetypes",
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
                            request.Filters ?? new FgsSetupLaborRateTypeListFilters(),
                            cancellationToken);
                    },
                    cancellationToken: cancellationToken);

                return ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>.Ok(cached!);
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
                request.Filters ?? new FgsSetupLaborRateTypeListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupLaborRateTypeSummaryDto>>(ex);
        }
    }
}
