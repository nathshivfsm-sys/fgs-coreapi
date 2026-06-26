using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Commands.UpdateFgsVehicleMaintenance;

public sealed class UpdateFgsVehicleMaintenanceCommandHandler(
    IFgsVehicleMaintenanceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsVehicleMaintenanceCommandHandler> logger)
    : IRequestHandler<UpdateFgsVehicleMaintenanceCommand, ApiResponse<FgsVehicleMaintenanceDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleMaintenanceDetailDto>> Handle(
        UpdateFgsVehicleMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated vehicle maintenance {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "vehiclemaintenances"),
                cancellationToken);
        return ApiResponse<FgsVehicleMaintenanceDetailDto>.Ok(result);
    }
}
