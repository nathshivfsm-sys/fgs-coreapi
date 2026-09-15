using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloLanguages;

public sealed class LookupGloLanguagesQueryHandler(
    IGloLanguageReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloLanguagesQuery, ApiResponse<IReadOnlyList<GloLanguageLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloLanguageLookupDto>>> Handle(
        LookupGloLanguagesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "language",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloLanguageLookupDto>>.Ok(result ?? Array.Empty<GloLanguageLookupDto>());
    }
}
