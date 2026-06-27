using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.LookupLeadDisqualificationReasons;

public sealed class LookupLeadDisqualificationReasonsQueryHandler(
    ILeadDisqualificationReasonReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupLeadDisqualificationReasonsQuery, ApiResponse<IReadOnlyList<LeadDisqualificationReasonLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<LeadDisqualificationReasonLookupDto>>> Handle(
        LookupLeadDisqualificationReasonsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "leaddisqualificationreasons",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<LeadDisqualificationReasonLookupDto>>.Ok(result ?? Array.Empty<LeadDisqualificationReasonLookupDto>());
    }
}
