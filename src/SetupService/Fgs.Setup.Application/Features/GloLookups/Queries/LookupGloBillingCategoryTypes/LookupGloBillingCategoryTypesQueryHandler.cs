using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloBillingCategoryTypes;

public sealed class LookupGloBillingCategoryTypesQueryHandler(
    IGloBillingCategoryReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloBillingCategoryTypesQuery, ApiResponse<IReadOnlyList<GloBillingCategoryTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloBillingCategoryTypeLookupDto>>> Handle(
        LookupGloBillingCategoryTypesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "billingcategorytype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloBillingCategoryTypeLookupDto>>.Ok(
            result ?? Array.Empty<GloBillingCategoryTypeLookupDto>());
    }
}
