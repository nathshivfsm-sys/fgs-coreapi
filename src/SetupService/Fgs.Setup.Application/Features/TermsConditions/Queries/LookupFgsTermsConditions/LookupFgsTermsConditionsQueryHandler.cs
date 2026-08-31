using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.TermsConditions;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TermsConditions.Queries.LookupFgsTermsConditions;

public sealed class LookupFgsTermsConditionsQueryHandler(
    IFgsTermsConditionReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupFgsTermsConditionsQuery, ApiResponse<IReadOnlyList<FgsTermsConditionLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsTermsConditionLookupDto>>> Handle(
        LookupFgsTermsConditionsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "termscondition",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsTermsConditionLookupDto>>.Ok(
            result ?? Array.Empty<FgsTermsConditionLookupDto>());
    }
}
