using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Inventory.Application.Abstractions.InventorySerials;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventorySerials.Commands.CreateFgsInventorySerial;

public sealed class CreateFgsInventorySerialCommandHandler(
    IFgsInventorySerialWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInventorySerialCommandHandler> logger)
    : IRequestHandler<CreateFgsInventorySerialCommand, ApiResponse<FgsInventorySerialDetailDto>>
{
    public async Task<ApiResponse<FgsInventorySerialDetailDto>> Handle(
        CreateFgsInventorySerialCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created inventory serial {Id} with serial number {SerialNumber}", result.Id, result.SerialNumber);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryserial"),
            cancellationToken);
        return ApiResponse<FgsInventorySerialDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
