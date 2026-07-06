using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Queries.GetFgsAssetManufacturerById;

public sealed class GetFgsAssetManufacturerByIdQueryHandler(
    IFgsAssetManufacturerReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsAssetManufacturerByIdQuery, ApiResponse<FgsAssetManufacturerDetailDto>>
{
    public async Task<ApiResponse<FgsAssetManufacturerDetailDto>> Handle(
        GetFgsAssetManufacturerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "assetmanufacturer",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsAssetManufacturerDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsAssetManufacturerDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsAssetManufacturerDetailDto>.Fail(
                [$"Asset Manufacturer '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsAssetManufacturerDetailDto>.Ok(result);
    }
}
