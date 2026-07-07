using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Commands.PatchFgsAssetStatus;

public sealed class PatchFgsAssetStatusCommandHandler(
    IFgsAssetStatusWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<PatchFgsAssetStatusCommand, ApiResponse<FgsAssetStatusDetailDto>>
{
    public async Task<ApiResponse<FgsAssetStatusDetailDto>> Handle(
        PatchFgsAssetStatusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetstatus"),
            cancellationToken);
        return ApiResponse<FgsAssetStatusDetailDto>.Ok(result);
    }
}
