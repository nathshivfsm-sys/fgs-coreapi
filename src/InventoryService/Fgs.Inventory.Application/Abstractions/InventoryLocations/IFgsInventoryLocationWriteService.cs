using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryLocations;

public interface IFgsInventoryLocationWriteService
{
    Task<FgsInventoryLocationDetailDto> CreateAsync(
        FgsInventoryLocationCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsInventoryLocationDetailDto> UpdateAsync(
        long id,
        FgsInventoryLocationUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsInventoryLocationDetailDto> PatchAsync(
        long id,
        FgsInventoryLocationPatchDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsInventoryLocationDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
