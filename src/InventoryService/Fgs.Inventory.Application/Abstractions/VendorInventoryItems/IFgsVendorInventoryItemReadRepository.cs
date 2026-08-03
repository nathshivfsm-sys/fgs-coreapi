using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.VendorInventoryItems;

public interface IFgsVendorInventoryItemReadRepository
{
    Task<FgsVendorInventoryItemDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsVendorInventoryItemSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsVendorInventoryItemListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsVendorInventoryItemLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByVendorAndItemAsync(
        long vendorId,
        long inventoryItemId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default);
}
