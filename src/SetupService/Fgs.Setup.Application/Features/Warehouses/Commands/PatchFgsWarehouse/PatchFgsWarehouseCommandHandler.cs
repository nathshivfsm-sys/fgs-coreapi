using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Warehouses.Commands.PatchFgsWarehouse;

public sealed class PatchFgsWarehouseCommandHandler(
    IFgsWarehouseWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsWarehouseCommandHandler> logger)
    : IRequestHandler<PatchFgsWarehouseCommand, ApiResponse<FgsWarehouseDetailDto>>
{
    public async Task<ApiResponse<FgsWarehouseDetailDto>> Handle(
        PatchFgsWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd warehouse {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current;
            if (tenantScope?.IsResolved == true)
            {
                await cache.RemoveByPrefixAsync(
                    CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "warehouses"),
                    cancellationToken);
            }
            return ApiResponse<FgsWarehouseDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch warehouse {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsWarehouseDetailDto>(ex);
        }
    }
}
