using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventorySerials;

public interface IFgsInventorySerialReadRepository
{
    Task<FgsInventorySerialDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsInventorySerialSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventorySerialListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsInventorySerialLookupDto>> LookupAsync(
        long? inventoryItemId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsBySerialNumberAsync(
        long inventoryItemId,
        string serialNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default);
}
