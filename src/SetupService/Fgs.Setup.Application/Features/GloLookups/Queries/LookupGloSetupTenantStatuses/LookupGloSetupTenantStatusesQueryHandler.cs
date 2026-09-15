using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloSetupTenantStatuses;

public sealed class LookupGloSetupTenantStatusesQueryHandler(
    IGloSetupTenantStatusReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloSetupTenantStatusesQuery, ApiResponse<IReadOnlyList<GloSetupTenantStatusLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloSetupTenantStatusLookupDto>>> Handle(
        LookupGloSetupTenantStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "setuptenantstatus",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloSetupTenantStatusLookupDto>>.Ok(result ?? Array.Empty<GloSetupTenantStatusLookupDto>());
    }
}
