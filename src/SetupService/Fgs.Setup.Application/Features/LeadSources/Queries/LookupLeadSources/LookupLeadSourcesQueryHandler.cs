using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Queries.LookupLeadSources;

public sealed class LookupLeadSourcesQueryHandler(
    ILeadSourceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupLeadSourcesQuery, ApiResponse<IReadOnlyList<LeadSourceLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<LeadSourceLookupDto>>> Handle(
        LookupLeadSourcesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "leadsources",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<LeadSourceLookupDto>>.Ok(result ?? Array.Empty<LeadSourceLookupDto>());
    }
}
