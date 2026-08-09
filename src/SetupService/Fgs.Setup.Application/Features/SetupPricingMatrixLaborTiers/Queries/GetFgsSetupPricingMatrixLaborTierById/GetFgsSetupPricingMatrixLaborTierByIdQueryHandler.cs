using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.GetFgsSetupPricingMatrixLaborTierById;

public sealed class GetFgsSetupPricingMatrixLaborTierByIdQueryHandler(IFgsSetupPricingMatrixLaborTierReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<GetFgsSetupPricingMatrixLaborTierByIdQuery, ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>> Handle(GetFgsSetupPricingMatrixLaborTierByIdQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixlabortier", request.Id.ToString());
        var cached = await cache.GetAsync<FgsSetupPricingMatrixLaborTierDetailDto>(key, cancellationToken);
        if (cached is not null) return ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>.Ok(cached);
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null) return ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>.Fail(["Pricing Matrix Labor Tier '" + request.Id + "' was not found."], ApiStatusCodes.NotFound);
        await cache.SetAsync(key, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>.Ok(result);
    }
}
