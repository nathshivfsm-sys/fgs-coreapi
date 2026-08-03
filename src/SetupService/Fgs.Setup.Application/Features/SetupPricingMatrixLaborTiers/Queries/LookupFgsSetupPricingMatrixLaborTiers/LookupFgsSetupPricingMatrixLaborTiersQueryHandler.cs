using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.LookupFgsSetupPricingMatrixLaborTiers;

public sealed class LookupFgsSetupPricingMatrixLaborTiersQueryHandler(IFgsSetupPricingMatrixLaborTierReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<LookupFgsSetupPricingMatrixLaborTiersQuery, ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLaborTierLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLaborTierLookupDto>>> Handle(LookupFgsSetupPricingMatrixLaborTiersQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixlabortier", $"{CacheKeys.LookupSegment(request.ActiveOnly)}:parent:{request.PricingMatrixLaborId}");
        var result = await cache.GetOrSetAsync(key, () => readRepository.LookupAsync(request.ActiveOnly, request.PricingMatrixLaborId, cancellationToken), cancellationToken: cancellationToken);
        return ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLaborTierLookupDto>>.Ok(result ?? Array.Empty<FgsSetupPricingMatrixLaborTierLookupDto>());
    }
}
