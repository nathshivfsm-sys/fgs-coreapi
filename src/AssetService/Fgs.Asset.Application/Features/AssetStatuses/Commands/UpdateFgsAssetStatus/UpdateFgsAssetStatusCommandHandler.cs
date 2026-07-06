using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.AssetStatuses.Commands.UpdateFgsAssetStatus;

public sealed class UpdateFgsAssetStatusCommandHandler(
    IFgsAssetStatusWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsAssetStatusCommandHandler> logger)
    : IRequestHandler<UpdateFgsAssetStatusCommand, ApiResponse<FgsAssetStatusDetailDto>>
{
    public async Task<ApiResponse<FgsAssetStatusDetailDto>> Handle(
        UpdateFgsAssetStatusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetstatus"),
            cancellationToken);
        return ApiResponse<FgsAssetStatusDetailDto>.Ok(result);
    }
}
