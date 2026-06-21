using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Commands.DeleteFgsVehicleMaintenance;

public sealed class DeleteFgsVehicleMaintenanceCommandHandler(
    IFgsVehicleMaintenanceWriteService writeService,
    ILogger<DeleteFgsVehicleMaintenanceCommandHandler> logger)
    : IRequestHandler<DeleteFgsVehicleMaintenanceCommand, ApiResponse<FgsVehicleMaintenanceDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleMaintenanceDetailDto>> Handle(
        DeleteFgsVehicleMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted vehicle maintenance {Id}", result.Id);
            return ApiResponse<FgsVehicleMaintenanceDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete vehicle maintenance {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsVehicleMaintenanceDetailDto>(ex);
        }
    }
}
