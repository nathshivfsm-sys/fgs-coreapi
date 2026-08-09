using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryStocks;

public interface IFgsInventoryStockReadRepository
{
    Task<FgsInventoryStockDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsInventoryStockSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryStockListFilters filters,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByInventoryItemIdAsync(
        long inventoryItemId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default);
}
