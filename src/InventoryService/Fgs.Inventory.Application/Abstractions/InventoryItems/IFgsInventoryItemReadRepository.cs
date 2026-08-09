using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryItems;

public interface IFgsInventoryItemReadRepository
{
    Task<FgsInventoryItemDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsInventoryItemSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryItemListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsInventoryItemLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByItemCodeAsync(
        string itemCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default);
}
