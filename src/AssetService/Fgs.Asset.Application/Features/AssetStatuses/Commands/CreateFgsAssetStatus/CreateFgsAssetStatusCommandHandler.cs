using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Commands.CreateFgsAssetStatus;

public sealed class CreateFgsAssetStatusCommandHandler(
    IFgsAssetStatusWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<CreateFgsAssetStatusCommand, ApiResponse<FgsAssetStatusDetailDto>>
{
    public async Task<ApiResponse<FgsAssetStatusDetailDto>> Handle(
        CreateFgsAssetStatusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetstatus"),
            cancellationToken);
        return ApiResponse<FgsAssetStatusDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
