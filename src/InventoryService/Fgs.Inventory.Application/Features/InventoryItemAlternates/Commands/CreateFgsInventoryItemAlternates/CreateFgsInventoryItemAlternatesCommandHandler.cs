using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Inventory.Application.Abstractions.InventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.CreateFgsInventoryItemAlternates;

public sealed class CreateFgsInventoryItemAlternatesCommandHandler(
    IFgsInventoryItemAlternateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInventoryItemAlternatesCommandHandler> logger)
    : IRequestHandler<CreateFgsInventoryItemAlternatesCommand, ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>> Handle(
        CreateFgsInventoryItemAlternatesCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.ReplaceAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Replaced alternates for inventory item {InventoryItemId}; count {Count}",
            request.Dto.InventoryItemId,
            result.Count);
        await InvalidateCacheAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>.Ok(result, ApiStatusCodes.Created);
    }

    private async Task InvalidateCacheAsync(CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryitem"),
            cancellationToken);
    }
}
