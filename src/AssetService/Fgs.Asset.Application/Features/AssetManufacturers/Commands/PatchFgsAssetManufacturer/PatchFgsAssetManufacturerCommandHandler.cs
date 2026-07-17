using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Commands.PatchFgsAssetManufacturer;

public sealed class PatchFgsAssetManufacturerCommandHandler(
    IFgsAssetManufacturerWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<PatchFgsAssetManufacturerCommand, ApiResponse<FgsAssetManufacturerDetailDto>>
{
    public async Task<ApiResponse<FgsAssetManufacturerDetailDto>> Handle(
        PatchFgsAssetManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetmanufacturer"),
            cancellationToken);
        return ApiResponse<FgsAssetManufacturerDetailDto>.Ok(result);
    }
}
