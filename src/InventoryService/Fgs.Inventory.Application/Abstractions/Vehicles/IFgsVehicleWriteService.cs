using Fgs.Inventory.Application.Features.Vehicles.Dtos;

namespace Fgs.Inventory.Application.Abstractions.Vehicles;

public interface IFgsVehicleWriteService
{
    Task<FgsVehicleDetailDto> CreateAsync(FgsVehicleCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsVehicleDetailDto> UpdateAsync(long id, FgsVehicleUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsVehicleDetailDto> PatchAsync(long id, FgsVehiclePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsVehicleDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
