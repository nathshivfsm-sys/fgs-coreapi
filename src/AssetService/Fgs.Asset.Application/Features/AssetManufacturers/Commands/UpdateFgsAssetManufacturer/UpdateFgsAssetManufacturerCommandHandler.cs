using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Commands.UpdateFgsAssetManufacturer;

public sealed class UpdateFgsAssetManufacturerCommandHandler(
    IFgsAssetManufacturerWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsAssetManufacturerCommandHandler> logger)
    : IRequestHandler<UpdateFgsAssetManufacturerCommand, ApiResponse<FgsAssetManufacturerDetailDto>>
{
    public async Task<ApiResponse<FgsAssetManufacturerDetailDto>> Handle(
        UpdateFgsAssetManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetmanufacturer"),
            cancellationToken);
        return ApiResponse<FgsAssetManufacturerDetailDto>.Ok(result);
    }
}
