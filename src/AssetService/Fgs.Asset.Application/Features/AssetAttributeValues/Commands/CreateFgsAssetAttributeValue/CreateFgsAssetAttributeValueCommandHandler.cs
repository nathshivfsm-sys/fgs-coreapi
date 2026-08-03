using Fgs.Asset.Application.Abstractions.AssetAttributeValues;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Commands.CreateFgsAssetAttributeValue;

public sealed class CreateFgsAssetAttributeValueCommandHandler(
    IFgsAssetAttributeValueWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<CreateFgsAssetAttributeValueCommand, ApiResponse<FgsAssetAttributeValueDetailDto>>
{
    public async Task<ApiResponse<FgsAssetAttributeValueDetailDto>> Handle(
        CreateFgsAssetAttributeValueCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetattributevalue"),
            cancellationToken);
        return ApiResponse<FgsAssetAttributeValueDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
