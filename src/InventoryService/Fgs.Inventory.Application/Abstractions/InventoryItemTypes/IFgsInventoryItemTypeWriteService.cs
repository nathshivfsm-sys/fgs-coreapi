using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryItemTypes;

public interface IFgsInventoryItemTypeWriteService
{
    Task<FgsInventoryItemTypeDetailDto> CreateAsync(FgsInventoryItemTypeCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInventoryItemTypeDetailDto> UpdateAsync(long id, FgsInventoryItemTypeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInventoryItemTypeDetailDto> PatchAsync(long id, FgsInventoryItemTypePatchDto dto, CancellationToken cancellationToken = default);
}
