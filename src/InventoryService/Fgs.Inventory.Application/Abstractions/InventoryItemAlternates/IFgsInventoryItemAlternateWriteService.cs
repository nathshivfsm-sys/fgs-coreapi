using Fgs.Inventory.Application.Features.InventoryItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryItemAlternates;

public interface IFgsInventoryItemAlternateWriteService
{
    Task<IReadOnlyList<FgsInventoryItemAlternateDetailDto>> ReplaceAsync(
        FgsInventoryItemAlternateReplaceDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
