using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Commands.CreateFgsInventoryLocation;

public sealed class CreateFgsInventoryLocationCommandHandler(
    IFgsInventoryLocationWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInventoryLocationCommandHandler> logger)
    : IRequestHandler<CreateFgsInventoryLocationCommand, ApiResponse<FgsInventoryLocationDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryLocationDetailDto>> Handle(
        CreateFgsInventoryLocationCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created inventory location {Id} with code {InventoryLocationCode}", result.Id, result.InventoryLocationCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorylocation"),
            cancellationToken);
        return ApiResponse<FgsInventoryLocationDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
