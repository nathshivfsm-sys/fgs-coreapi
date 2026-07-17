using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Commands.UpdateFgsAssetAttribute;

public sealed class UpdateFgsAssetAttributeCommandHandler(
    IFgsAssetAttributeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<UpdateFgsAssetAttributeCommand, ApiResponse<FgsAssetAttributeDetailDto>>
{
    public async Task<ApiResponse<FgsAssetAttributeDetailDto>> Handle(
        UpdateFgsAssetAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetattribute"),
            cancellationToken);
        return ApiResponse<FgsAssetAttributeDetailDto>.Ok(result);
    }
}
