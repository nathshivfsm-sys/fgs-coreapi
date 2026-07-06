using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.LookupFgsBusinessTypes;

public sealed class LookupFgsBusinessTypesQueryHandler(
    IFgsBusinessTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupFgsBusinessTypesQuery, ApiResponse<IReadOnlyList<FgsBusinessTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsBusinessTypeLookupDto>>> Handle(
        LookupFgsBusinessTypesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "businesstype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsBusinessTypeLookupDto>>.Ok(result ?? Array.Empty<FgsBusinessTypeLookupDto>());
    }
}
