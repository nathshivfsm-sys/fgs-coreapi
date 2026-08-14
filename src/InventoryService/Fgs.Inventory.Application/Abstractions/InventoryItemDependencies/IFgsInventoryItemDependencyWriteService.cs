using Fgs.Inventory.Application.Features.InventoryItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryItemDependencies;

public interface IFgsInventoryItemDependencyWriteService
{
    Task<IReadOnlyList<FgsInventoryItemDependencyDetailDto>> ReplaceAsync(
        FgsInventoryItemDependencyReplaceDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
