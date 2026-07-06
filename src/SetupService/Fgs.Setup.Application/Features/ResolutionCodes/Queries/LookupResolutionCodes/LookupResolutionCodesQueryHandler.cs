using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.LookupResolutionCodes;

public sealed class LookupResolutionCodesQueryHandler(
    IResolutionCodeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupResolutionCodesQuery, ApiResponse<IReadOnlyList<ResolutionCodeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<ResolutionCodeLookupDto>>> Handle(
        LookupResolutionCodesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "resolutioncode",
            $"{CacheKeys.LookupSegment(request.ActiveOnly)}:isMobileVisible={request.IsMobileVisible?.ToString() ?? "all"}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, request.IsMobileVisible, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<ResolutionCodeLookupDto>>.Ok(result ?? Array.Empty<ResolutionCodeLookupDto>());
    }
}
