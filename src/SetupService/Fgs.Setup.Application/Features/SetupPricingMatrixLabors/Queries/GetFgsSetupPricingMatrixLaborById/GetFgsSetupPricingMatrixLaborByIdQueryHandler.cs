using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.GetFgsSetupPricingMatrixLaborById;

public sealed class GetFgsSetupPricingMatrixLaborByIdQueryHandler(IFgsSetupPricingMatrixLaborReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<GetFgsSetupPricingMatrixLaborByIdQuery, ApiResponse<FgsSetupPricingMatrixLaborDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixLaborDetailDto>> Handle(GetFgsSetupPricingMatrixLaborByIdQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixlabor", request.Id.ToString());
        var cached = await cache.GetAsync<FgsSetupPricingMatrixLaborDetailDto>(key, cancellationToken);
        if (cached is not null) return ApiResponse<FgsSetupPricingMatrixLaborDetailDto>.Ok(cached);
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null) return ApiResponse<FgsSetupPricingMatrixLaborDetailDto>.Fail(["Pricing Matrix Labor '" + request.Id + "' was not found."], ApiStatusCodes.NotFound);
        await cache.SetAsync(key, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixLaborDetailDto>.Ok(result);
    }
}
