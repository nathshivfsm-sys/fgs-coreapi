using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Queries.LookupSources;

public sealed class LookupSourcesQueryHandler(
    ISourceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSourcesQuery, ApiResponse<IReadOnlyList<SourceLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<SourceLookupDto>>> Handle(
        LookupSourcesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "source",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<SourceLookupDto>>.Ok(result ?? Array.Empty<SourceLookupDto>());
    }
}
