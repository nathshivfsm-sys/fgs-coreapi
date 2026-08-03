using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.PatchFgsVendorInventoryItem;

public sealed class PatchFgsVendorInventoryItemCommandHandler(
    IFgsVendorInventoryItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsVendorInventoryItemCommandHandler> logger)
    : IRequestHandler<PatchFgsVendorInventoryItemCommand, ApiResponse<FgsVendorInventoryItemDetailDto>>
{
    public async Task<ApiResponse<FgsVendorInventoryItemDetailDto>> Handle(
        PatchFgsVendorInventoryItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched vendor inventory item {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "vendorinventoryitem"),
            cancellationToken);
        return ApiResponse<FgsVendorInventoryItemDetailDto>.Ok(result);
    }
}
