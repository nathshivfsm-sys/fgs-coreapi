using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloAccountingIntegrationTypes;

public sealed class LookupGloAccountingIntegrationTypesQueryHandler(
    IGloAccountingIntegrationTypeReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloAccountingIntegrationTypesQuery, ApiResponse<IReadOnlyList<GloAccountingIntegrationTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloAccountingIntegrationTypeLookupDto>>> Handle(
        LookupGloAccountingIntegrationTypesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "accountingintegrationtype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloAccountingIntegrationTypeLookupDto>>.Ok(result ?? Array.Empty<GloAccountingIntegrationTypeLookupDto>());
    }
}
