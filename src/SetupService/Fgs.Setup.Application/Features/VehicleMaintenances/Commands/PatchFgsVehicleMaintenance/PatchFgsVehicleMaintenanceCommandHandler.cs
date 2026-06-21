using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Commands.PatchFgsVehicleMaintenance;

public sealed class PatchFgsVehicleMaintenanceCommandHandler(
    IFgsVehicleMaintenanceWriteService writeService,
    ILogger<PatchFgsVehicleMaintenanceCommandHandler> logger)
    : IRequestHandler<PatchFgsVehicleMaintenanceCommand, ApiResponse<FgsVehicleMaintenanceDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleMaintenanceDetailDto>> Handle(
        PatchFgsVehicleMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd vehicle maintenance {Id}", result.Id);
            return ApiResponse<FgsVehicleMaintenanceDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch vehicle maintenance {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsVehicleMaintenanceDetailDto>(ex);
        }
    }
}
