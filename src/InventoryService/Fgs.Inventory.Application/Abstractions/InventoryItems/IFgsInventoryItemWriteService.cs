using Fgs.Inventory.Application.Features.InventoryItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryItems;

public interface IFgsInventoryItemWriteService
{
    Task<FgsInventoryItemDetailDto> CreateAsync(FgsInventoryItemCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInventoryItemDetailDto> UpdateAsync(long id, FgsInventoryItemUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInventoryItemDetailDto> PatchAsync(long id, FgsInventoryItemPatchDto dto, CancellationToken cancellationToken = default);
}
