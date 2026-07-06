using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.LookupSalesActivityTypes;

public sealed class LookupSalesActivityTypesQueryHandler(
    IFgsSalesActivityTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSalesActivityTypesQuery, ApiResponse<IReadOnlyList<FgsSalesActivityTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSalesActivityTypeLookupDto>>> Handle(
        LookupSalesActivityTypesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "salesactivitytype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSalesActivityTypeLookupDto>>.Ok(result ?? Array.Empty<FgsSalesActivityTypeLookupDto>());
    }
}
