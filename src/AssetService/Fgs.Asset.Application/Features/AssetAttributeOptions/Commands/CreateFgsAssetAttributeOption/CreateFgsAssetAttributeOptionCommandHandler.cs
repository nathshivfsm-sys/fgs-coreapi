using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.CreateFgsAssetAttributeOption;

public sealed class CreateFgsAssetAttributeOptionCommandHandler(
    IFgsAssetAttributeOptionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<CreateFgsAssetAttributeOptionCommand, ApiResponse<FgsAssetAttributeOptionDetailDto>>
{
    public async Task<ApiResponse<FgsAssetAttributeOptionDetailDto>> Handle(
        CreateFgsAssetAttributeOptionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetattributeoption"),
            cancellationToken);
        return ApiResponse<FgsAssetAttributeOptionDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
