using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;

public interface IFgsTruckStockTemplateItemReadRepository
{
    Task<FgsTruckStockTemplateItemDetailDto?> GetByIdAsync(
        long templateId,
        long itemId,
        CancellationToken cancellationToken = default);

    Task<PagedResult<FgsTruckStockTemplateItemSummaryDto>> ListByTemplateAsync(
        long templateId,
        InventoryListQuery query,
        FgsTruckStockTemplateItemListFilters filters,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByTemplateAndItemAsync(
        long templateId,
        long inventoryItemId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default);
}
