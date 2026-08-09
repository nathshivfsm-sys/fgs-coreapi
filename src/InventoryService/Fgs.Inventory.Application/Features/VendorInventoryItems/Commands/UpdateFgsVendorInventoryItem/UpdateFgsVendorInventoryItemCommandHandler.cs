using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.UpdateFgsVendorInventoryItem;

public sealed class UpdateFgsVendorInventoryItemCommandHandler(
    IFgsVendorInventoryItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsVendorInventoryItemCommandHandler> logger)
    : IRequestHandler<UpdateFgsVendorInventoryItemCommand, ApiResponse<FgsVendorInventoryItemDetailDto>>
{
    public async Task<ApiResponse<FgsVendorInventoryItemDetailDto>> Handle(
        UpdateFgsVendorInventoryItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated vendor inventory item {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "vendorinventoryitem"),
            cancellationToken);
        return ApiResponse<FgsVendorInventoryItemDetailDto>.Ok(result);
    }
}
