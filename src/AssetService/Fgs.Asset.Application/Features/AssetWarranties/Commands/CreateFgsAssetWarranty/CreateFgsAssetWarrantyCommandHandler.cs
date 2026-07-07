using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Commands.CreateFgsAssetWarranty;

public sealed class CreateFgsAssetWarrantyCommandHandler(
    IFgsAssetWarrantyWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<CreateFgsAssetWarrantyCommand, ApiResponse<FgsAssetWarrantyDetailDto>>
{
    public async Task<ApiResponse<FgsAssetWarrantyDetailDto>> Handle(
        CreateFgsAssetWarrantyCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetwarranty"),
            cancellationToken);
        return ApiResponse<FgsAssetWarrantyDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
