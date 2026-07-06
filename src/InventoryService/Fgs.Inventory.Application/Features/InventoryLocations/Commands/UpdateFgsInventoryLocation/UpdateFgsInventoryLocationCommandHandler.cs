using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Commands.UpdateFgsInventoryLocation;

public sealed class UpdateFgsInventoryLocationCommandHandler(
    IFgsInventoryLocationWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsInventoryLocationCommandHandler> logger)
    : IRequestHandler<UpdateFgsInventoryLocationCommand, ApiResponse<FgsInventoryLocationDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryLocationDetailDto>> Handle(
        UpdateFgsInventoryLocationCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated inventory location {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventory-location"),
            cancellationToken);
        return ApiResponse<FgsInventoryLocationDetailDto>.Ok(result);
    }
}
