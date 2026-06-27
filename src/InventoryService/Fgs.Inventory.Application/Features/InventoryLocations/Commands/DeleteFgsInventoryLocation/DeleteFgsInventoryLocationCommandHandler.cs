using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Commands.DeleteFgsInventoryLocation;

public sealed class DeleteFgsInventoryLocationCommandHandler(
    IFgsInventoryLocationWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsInventoryLocationCommandHandler> logger)
    : IRequestHandler<DeleteFgsInventoryLocationCommand, ApiResponse<FgsInventoryLocationDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryLocationDetailDto>> Handle(
        DeleteFgsInventoryLocationCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted inventory location {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventory-locations"),
            cancellationToken);
        return ApiResponse<FgsInventoryLocationDetailDto>.Ok(result);
    }
}
