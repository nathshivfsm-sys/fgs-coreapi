using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Inventory.Application.Abstractions.InventorySerials;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventorySerials.Commands.PatchFgsInventorySerial;

public sealed class PatchFgsInventorySerialCommandHandler(
    IFgsInventorySerialWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsInventorySerialCommandHandler> logger)
    : IRequestHandler<PatchFgsInventorySerialCommand, ApiResponse<FgsInventorySerialDetailDto>>
{
    public async Task<ApiResponse<FgsInventorySerialDetailDto>> Handle(
        PatchFgsInventorySerialCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched inventory serial {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryserial"),
            cancellationToken);
        return ApiResponse<FgsInventorySerialDetailDto>.Ok(result);
    }
}
