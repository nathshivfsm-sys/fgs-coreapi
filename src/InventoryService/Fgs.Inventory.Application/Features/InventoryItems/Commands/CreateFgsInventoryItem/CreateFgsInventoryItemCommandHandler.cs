using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryItems.Commands.CreateFgsInventoryItem;

public sealed class CreateFgsInventoryItemCommandHandler(
    IFgsInventoryItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInventoryItemCommandHandler> logger)
    : IRequestHandler<CreateFgsInventoryItemCommand, ApiResponse<FgsInventoryItemDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryItemDetailDto>> Handle(
        CreateFgsInventoryItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created inventory item {Id} with code {ItemCode}", result.Id, result.ItemCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryitem"),
            cancellationToken);
        return ApiResponse<FgsInventoryItemDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
