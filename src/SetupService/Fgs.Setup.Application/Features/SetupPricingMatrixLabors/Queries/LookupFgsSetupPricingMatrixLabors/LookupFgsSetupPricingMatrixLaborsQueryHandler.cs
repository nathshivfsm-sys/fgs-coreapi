using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.LookupFgsSetupPricingMatrixLabors;

public sealed class LookupFgsSetupPricingMatrixLaborsQueryHandler(IFgsSetupPricingMatrixLaborReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<LookupFgsSetupPricingMatrixLaborsQuery, ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLaborLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLaborLookupDto>>> Handle(LookupFgsSetupPricingMatrixLaborsQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixlabor", $"{CacheKeys.LookupSegment(request.ActiveOnly)}:parent:{request.PricingMatrixId}");
        var result = await cache.GetOrSetAsync(key, () => readRepository.LookupAsync(request.ActiveOnly, request.PricingMatrixId, cancellationToken), cancellationToken: cancellationToken);
        return ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLaborLookupDto>>.Ok(result ?? Array.Empty<FgsSetupPricingMatrixLaborLookupDto>());
    }
}
