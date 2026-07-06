using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.LookupLeadStatuses;

public sealed class LookupLeadStatusesQueryHandler(
    ILeadStatusReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupLeadStatusesQuery, ApiResponse<IReadOnlyList<LeadStatusLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<LeadStatusLookupDto>>> Handle(
        LookupLeadStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "leadstatus",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<LeadStatusLookupDto>>.Ok(result ?? Array.Empty<LeadStatusLookupDto>());
    }
}
