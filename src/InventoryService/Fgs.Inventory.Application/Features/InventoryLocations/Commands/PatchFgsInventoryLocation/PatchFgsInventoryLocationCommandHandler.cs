using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Commands.PatchFgsInventoryLocation;

public sealed class PatchFgsInventoryLocationCommandHandler(
    IFgsInventoryLocationWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsInventoryLocationCommandHandler> logger)
    : IRequestHandler<PatchFgsInventoryLocationCommand, ApiResponse<FgsInventoryLocationDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryLocationDetailDto>> Handle(
        PatchFgsInventoryLocationCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched inventory location {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventory-locations"),
            cancellationToken);
        return ApiResponse<FgsInventoryLocationDetailDto>.Ok(result);
    }
}
