using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.LookupSalesActivityOutcomes;

public sealed class LookupSalesActivityOutcomesQueryHandler(
    IFgsSalesActivityOutcomeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSalesActivityOutcomesQuery, ApiResponse<IReadOnlyList<FgsSalesActivityOutcomeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSalesActivityOutcomeLookupDto>>> Handle(
        LookupSalesActivityOutcomesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "salesactivityoutcome",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSalesActivityOutcomeLookupDto>>.Ok(result ?? Array.Empty<FgsSalesActivityOutcomeLookupDto>());
    }
}
