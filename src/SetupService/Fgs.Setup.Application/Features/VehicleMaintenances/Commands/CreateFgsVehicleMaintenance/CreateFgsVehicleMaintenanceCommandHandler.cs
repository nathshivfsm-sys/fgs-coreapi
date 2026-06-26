using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Commands.CreateFgsVehicleMaintenance;

public sealed class CreateFgsVehicleMaintenanceCommandHandler(
    IFgsVehicleMaintenanceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsVehicleMaintenanceCommandHandler> logger)
    : IRequestHandler<CreateFgsVehicleMaintenanceCommand, ApiResponse<FgsVehicleMaintenanceDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleMaintenanceDetailDto>> Handle(
        CreateFgsVehicleMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created vehicle maintenance {Id} with code {VehicleId}", result.Id, result.VehicleId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "vehiclemaintenances"),
                cancellationToken);
        return ApiResponse<FgsVehicleMaintenanceDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
