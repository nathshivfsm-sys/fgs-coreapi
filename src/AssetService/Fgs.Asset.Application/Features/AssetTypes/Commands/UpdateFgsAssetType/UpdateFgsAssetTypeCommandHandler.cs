using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Commands.UpdateFgsAssetType;

public sealed class UpdateFgsAssetTypeCommandHandler(
    IFgsAssetTypeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<UpdateFgsAssetTypeCommand, ApiResponse<FgsAssetTypeDetailDto>>
{
    public async Task<ApiResponse<FgsAssetTypeDetailDto>> Handle(
        UpdateFgsAssetTypeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assettype"),
            cancellationToken);
        return ApiResponse<FgsAssetTypeDetailDto>.Ok(result);
    }
}
