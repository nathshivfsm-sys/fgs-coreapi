using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.LookupSetupPaymentMethods;

public sealed class LookupSetupPaymentMethodsQueryHandler(
    IFgsSetupPaymentMethodReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupPaymentMethodsQuery, ApiResponse<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>> Handle(
        LookupSetupPaymentMethodsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "paymentmethod",
            $"{CacheKeys.LookupSegment(request.ActiveOnly)}:isMobileVisible={request.IsMobileVisible?.ToString() ?? "all"}:isCustomerPortalVisible={request.IsCustomerPortalVisible?.ToString() ?? "all"}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(
                request.ActiveOnly,
                request.IsMobileVisible,
                request.IsCustomerPortalVisible,
                cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>.Ok(result ?? Array.Empty<FgsSetupPaymentMethodLookupDto>());
    }
}
