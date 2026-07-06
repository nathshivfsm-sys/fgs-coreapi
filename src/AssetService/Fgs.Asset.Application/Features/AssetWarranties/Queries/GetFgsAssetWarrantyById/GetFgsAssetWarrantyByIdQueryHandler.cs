using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Queries.GetFgsAssetWarrantyById;

public sealed class GetFgsAssetWarrantyByIdQueryHandler(
    IFgsAssetWarrantyReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsAssetWarrantyByIdQuery, ApiResponse<FgsAssetWarrantyDetailDto>>
{
    public async Task<ApiResponse<FgsAssetWarrantyDetailDto>> Handle(
        GetFgsAssetWarrantyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "assetwarranty",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsAssetWarrantyDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsAssetWarrantyDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsAssetWarrantyDetailDto>.Fail(
                [$"Asset Warranty '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsAssetWarrantyDetailDto>.Ok(result);
    }
}
