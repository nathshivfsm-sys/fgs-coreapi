using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Queries.LookupFgsEntityDefaultTermsConditions;

public sealed class LookupFgsEntityDefaultTermsConditionsQueryHandler(
    IFgsEntityDefaultTermsConditionReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupFgsEntityDefaultTermsConditionsQuery, ApiResponse<IReadOnlyList<FgsEntityDefaultTermsConditionLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsEntityDefaultTermsConditionLookupDto>>> Handle(
        LookupFgsEntityDefaultTermsConditionsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "entitydefaulttermscondition",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsEntityDefaultTermsConditionLookupDto>>.Ok(
            result ?? Array.Empty<FgsEntityDefaultTermsConditionLookupDto>());
    }
}
