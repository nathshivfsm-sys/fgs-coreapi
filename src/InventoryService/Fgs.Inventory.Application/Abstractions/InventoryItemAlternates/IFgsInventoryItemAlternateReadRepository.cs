using Fgs.Inventory.Application.Features.InventoryItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryItemAlternates;

public interface IFgsInventoryItemAlternateReadRepository
{
    Task<FgsInventoryItemAlternateDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsInventoryItemAlternateDetailDto>> ListByInventoryItemIdAsync(
        long inventoryItemId,
        CancellationToken cancellationToken = default);
}
