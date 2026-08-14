using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Inventory.Application.Abstractions.InventoryItemDependencies;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.DeleteFgsInventoryItemDependency;

public sealed class DeleteFgsInventoryItemDependencyCommandHandler(
    IFgsInventoryItemDependencyWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsInventoryItemDependencyCommandHandler> logger)
    : IRequestHandler<DeleteFgsInventoryItemDependencyCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DeleteFgsInventoryItemDependencyCommand request,
        CancellationToken cancellationToken)
    {
        await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Deleted inventory item dependency {Id}", request.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryitem"),
            cancellationToken);
        return ApiResponse<object>.Ok(new { });
    }
}
