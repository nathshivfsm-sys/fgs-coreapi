using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloSetupDescriptionTypes;

public sealed class LookupGloSetupDescriptionTypesQueryHandler(
    IGloSetupDescriptionTypeReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloSetupDescriptionTypesQuery, ApiResponse<IReadOnlyList<GloSetupDescriptionTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloSetupDescriptionTypeLookupDto>>> Handle(
        LookupGloSetupDescriptionTypesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "setupdescriptiontype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloSetupDescriptionTypeLookupDto>>.Ok(result ?? Array.Empty<GloSetupDescriptionTypeLookupDto>());
    }
}
