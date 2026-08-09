using Fgs.Asset.Application.Abstractions.AssetAttributeValues;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Commands.PatchFgsAssetAttributeValue;

public sealed class PatchFgsAssetAttributeValueCommandHandler(
    IFgsAssetAttributeValueWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<PatchFgsAssetAttributeValueCommand, ApiResponse<FgsAssetAttributeValueDetailDto>>
{
    public async Task<ApiResponse<FgsAssetAttributeValueDetailDto>> Handle(
        PatchFgsAssetAttributeValueCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetattributevalue"),
            cancellationToken);
        return ApiResponse<FgsAssetAttributeValueDetailDto>.Ok(result);
    }
}
