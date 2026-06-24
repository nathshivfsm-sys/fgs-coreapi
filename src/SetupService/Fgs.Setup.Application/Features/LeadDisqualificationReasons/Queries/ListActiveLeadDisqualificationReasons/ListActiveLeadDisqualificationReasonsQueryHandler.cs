using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.ListActiveLeadDisqualificationReasons;

public sealed class ListActiveLeadDisqualificationReasonsQueryHandler(
    ILeadDisqualificationReasonReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveLeadDisqualificationReasonsQuery, ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>> Handle(
        ListActiveLeadDisqualificationReasonsQuery request,
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
                    "leaddisqualificationreasons",
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
                            request.Filters ?? new LeadDisqualificationReasonListFilters(),
                            cancellationToken);
                    },
                    cancellationToken: cancellationToken);

                return ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>.Ok(cached!);
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
                request.Filters ?? new LeadDisqualificationReasonListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<LeadDisqualificationReasonSummaryDto>>(ex);
        }
    }
}
