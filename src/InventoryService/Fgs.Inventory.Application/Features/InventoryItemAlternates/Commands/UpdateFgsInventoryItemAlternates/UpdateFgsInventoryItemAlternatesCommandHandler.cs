using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Inventory.Application.Abstractions.InventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.UpdateFgsInventoryItemAlternates;

public sealed class UpdateFgsInventoryItemAlternatesCommandHandler(
    IFgsInventoryItemAlternateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsInventoryItemAlternatesCommandHandler> logger)
    : IRequestHandler<UpdateFgsInventoryItemAlternatesCommand, ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>> Handle(
        UpdateFgsInventoryItemAlternatesCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.ReplaceAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Updated alternates for inventory item {InventoryItemId}; count {Count}",
            request.Dto.InventoryItemId,
            result.Count);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryitem"),
            cancellationToken);
        return ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>.Ok(result);
    }
}
