using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Inventory.Application.Abstractions.InventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.UpdateFgsInventoryItemDependencies;

public sealed class UpdateFgsInventoryItemDependenciesCommandHandler(
    IFgsInventoryItemDependencyWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsInventoryItemDependenciesCommandHandler> logger)
    : IRequestHandler<UpdateFgsInventoryItemDependenciesCommand, ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>> Handle(
        UpdateFgsInventoryItemDependenciesCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.ReplaceAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Updated dependencies for inventory item {InventoryItemId}; count {Count}",
            request.Dto.InventoryItemId,
            result.Count);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryitem"),
            cancellationToken);
        return ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>.Ok(result);
    }
}
