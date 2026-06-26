using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Commands.DeleteFgsVehicleMaintenance;

public sealed class DeleteFgsVehicleMaintenanceCommandHandler(
    IFgsVehicleMaintenanceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsVehicleMaintenanceCommandHandler> logger)
    : IRequestHandler<DeleteFgsVehicleMaintenanceCommand, ApiResponse<FgsVehicleMaintenanceDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleMaintenanceDetailDto>> Handle(
        DeleteFgsVehicleMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted vehicle maintenance {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "vehiclemaintenances"),
                cancellationToken);
        return ApiResponse<FgsVehicleMaintenanceDetailDto>.Ok(result);
    }
}
