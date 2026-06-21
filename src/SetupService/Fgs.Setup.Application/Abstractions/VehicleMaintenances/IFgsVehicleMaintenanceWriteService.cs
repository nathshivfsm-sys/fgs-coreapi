using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;

namespace Fgs.Setup.Application.Abstractions.VehicleMaintenances;

public interface IFgsVehicleMaintenanceWriteService
{
    Task<FgsVehicleMaintenanceDetailDto> CreateAsync(FgsVehicleMaintenanceCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsVehicleMaintenanceDetailDto> UpdateAsync(long id, FgsVehicleMaintenanceUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsVehicleMaintenanceDetailDto> PatchAsync(long id, FgsVehicleMaintenancePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsVehicleMaintenanceDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
