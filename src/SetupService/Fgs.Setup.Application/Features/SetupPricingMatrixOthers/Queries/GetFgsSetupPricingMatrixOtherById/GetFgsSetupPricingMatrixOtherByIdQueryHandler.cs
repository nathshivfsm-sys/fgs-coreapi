using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.GetFgsSetupPricingMatrixOtherById;

public sealed class GetFgsSetupPricingMatrixOtherByIdQueryHandler(IFgsSetupPricingMatrixOtherReadRepository readRepository, ICacheService cache, ITenantContextAccessor tenantContextAccessor) : IRequestHandler<GetFgsSetupPricingMatrixOtherByIdQuery, ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>> Handle(GetFgsSetupPricingMatrixOtherByIdQuery request, CancellationToken cancellationToken)
    {
        var scope = tenantContextAccessor.Current!;
        var key = CacheKeys.Build(scope.TenantId, scope.CompanyId, "pricingmatrixother", request.Id.ToString());
        var cached = await cache.GetAsync<FgsSetupPricingMatrixOtherDetailDto>(key, cancellationToken);
        if (cached is not null) return ApiResponse<FgsSetupPricingMatrixOtherDetailDto>.Ok(cached);
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null) return ApiResponse<FgsSetupPricingMatrixOtherDetailDto>.Fail(["Pricing Matrix Other '" + request.Id + "' was not found."], ApiStatusCodes.NotFound);
        await cache.SetAsync(key, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixOtherDetailDto>.Ok(result);
    }
}
