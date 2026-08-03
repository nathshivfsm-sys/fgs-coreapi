using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.CreateFgsVendorInventoryItem;

public sealed class CreateFgsVendorInventoryItemCommandHandler(
    IFgsVendorInventoryItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsVendorInventoryItemCommandHandler> logger)
    : IRequestHandler<CreateFgsVendorInventoryItemCommand, ApiResponse<FgsVendorInventoryItemDetailDto>>
{
    public async Task<ApiResponse<FgsVendorInventoryItemDetailDto>> Handle(
        CreateFgsVendorInventoryItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created vendor inventory item {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "vendorinventoryitem"),
            cancellationToken);
        return ApiResponse<FgsVendorInventoryItemDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
