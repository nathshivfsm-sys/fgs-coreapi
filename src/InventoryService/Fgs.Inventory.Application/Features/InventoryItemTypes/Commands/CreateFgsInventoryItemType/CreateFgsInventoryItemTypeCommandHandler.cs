using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.CreateFgsInventoryItemType;

public sealed class CreateFgsInventoryItemTypeCommandHandler(
    IFgsInventoryItemTypeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInventoryItemTypeCommandHandler> logger)
    : IRequestHandler<CreateFgsInventoryItemTypeCommand, ApiResponse<FgsInventoryItemTypeDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryItemTypeDetailDto>> Handle(
        CreateFgsInventoryItemTypeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created inventory item type {Id} with code {ItemTypeCode}", result.Id, result.ItemTypeCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryitemtype"),
            cancellationToken);
        return ApiResponse<FgsInventoryItemTypeDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
