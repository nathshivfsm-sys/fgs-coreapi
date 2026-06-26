using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.LookupSalesPipelineStatuses;

public sealed class LookupSalesPipelineStatusesQueryHandler(
    IFgsSalesPipelineStatusReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSalesPipelineStatusesQuery, ApiResponse<IReadOnlyList<FgsSalesPipelineStatusLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSalesPipelineStatusLookupDto>>> Handle(
        LookupSalesPipelineStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "salespipelinestatuses",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSalesPipelineStatusLookupDto>>.Ok(result ?? Array.Empty<FgsSalesPipelineStatusLookupDto>());
    }
}
