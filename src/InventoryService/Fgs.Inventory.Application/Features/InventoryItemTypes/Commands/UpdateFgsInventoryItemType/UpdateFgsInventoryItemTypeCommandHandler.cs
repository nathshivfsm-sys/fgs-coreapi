using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.UpdateFgsInventoryItemType;

public sealed class UpdateFgsInventoryItemTypeCommandHandler(
    IFgsInventoryItemTypeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsInventoryItemTypeCommandHandler> logger)
    : IRequestHandler<UpdateFgsInventoryItemTypeCommand, ApiResponse<FgsInventoryItemTypeDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryItemTypeDetailDto>> Handle(
        UpdateFgsInventoryItemTypeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated inventory item type {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryitemtype"),
            cancellationToken);
        return ApiResponse<FgsInventoryItemTypeDetailDto>.Ok(result);
    }
}
