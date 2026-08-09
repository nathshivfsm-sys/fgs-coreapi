using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;

namespace Fgs.Inventory.Application.Abstractions.PurchaseOrders;

public interface IFgsPurchaseOrderReadRepository
{
    Task<FgsPurchaseOrderDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsPurchaseOrderSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsPurchaseOrderListFilters filters,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByPurchaseOrderNumberAsync(
        string purchaseOrderNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryLocationAsync(long inventoryLocationId, CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default);
}
