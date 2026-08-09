using Fgs.Inventory.Application.Features.InventorySerials.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventorySerials;

public interface IFgsInventorySerialWriteService
{
    Task<FgsInventorySerialDetailDto> CreateAsync(
        FgsInventorySerialCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsInventorySerialDetailDto> UpdateAsync(
        long id,
        FgsInventorySerialUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsInventorySerialDetailDto> PatchAsync(
        long id,
        FgsInventorySerialPatchDto dto,
        CancellationToken cancellationToken = default);
}
