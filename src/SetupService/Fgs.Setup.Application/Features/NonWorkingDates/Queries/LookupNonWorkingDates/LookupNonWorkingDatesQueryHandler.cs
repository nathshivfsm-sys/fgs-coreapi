using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.NonWorkingDates;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Queries.LookupNonWorkingDates;

public sealed class LookupNonWorkingDatesQueryHandler(
    IFgsNonWorkingDateReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupNonWorkingDatesQuery, ApiResponse<IReadOnlyList<FgsNonWorkingDateLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsNonWorkingDateLookupDto>>> Handle(
        LookupNonWorkingDatesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "nonworkingdate",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsNonWorkingDateLookupDto>>.Ok(
            result ?? Array.Empty<FgsNonWorkingDateLookupDto>());
    }
}
