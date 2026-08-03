using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.LookupFgsSetupPricingMatrixOthers;

public sealed class LookupFgsSetupPricingMatrixOthersQueryHandler(IFgsSetupPricingMatrixOtherReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<LookupFgsSetupPricingMatrixOthersQuery, ApiResponse<IReadOnlyList<FgsSetupPricingMatrixOtherLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixOtherLookupDto>>> Handle(LookupFgsSetupPricingMatrixOthersQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixother", $"{CacheKeys.LookupSegment(request.ActiveOnly)}:parent:{request.PricingMatrixId}");
        var result = await cache.GetOrSetAsync(key, () => readRepository.LookupAsync(request.ActiveOnly, request.PricingMatrixId, cancellationToken), cancellationToken: cancellationToken);
        return ApiResponse<IReadOnlyList<FgsSetupPricingMatrixOtherLookupDto>>.Ok(result ?? Array.Empty<FgsSetupPricingMatrixOtherLookupDto>());
    }
}
