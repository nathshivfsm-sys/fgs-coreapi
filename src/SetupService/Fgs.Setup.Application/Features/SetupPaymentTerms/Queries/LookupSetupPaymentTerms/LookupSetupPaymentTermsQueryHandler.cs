using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.LookupSetupPaymentTerms;

public sealed class LookupSetupPaymentTermsQueryHandler(
    IFgsSetupPaymentTermReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupPaymentTermsQuery, ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>> Handle(
        LookupSetupPaymentTermsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "paymentterm",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>.Ok(result ?? Array.Empty<FgsSetupPaymentTermLookupDto>());
    }
}
