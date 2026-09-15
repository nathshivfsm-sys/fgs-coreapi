using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloMasterEntityTypes;

public sealed class LookupGloMasterEntityTypesQueryHandler(
    IGloMasterEntityTypeReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloMasterEntityTypesQuery, ApiResponse<IReadOnlyList<GloMasterEntityTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloMasterEntityTypeLookupDto>>> Handle(
        LookupGloMasterEntityTypesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "masterentitytype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloMasterEntityTypeLookupDto>>.Ok(result ?? Array.Empty<GloMasterEntityTypeLookupDto>());
    }
}
