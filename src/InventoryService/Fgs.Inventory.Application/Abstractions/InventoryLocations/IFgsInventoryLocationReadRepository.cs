using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryLocations;

public interface IFgsInventoryLocationReadRepository
{
    Task<FgsInventoryLocationDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsInventoryLocationSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryLocationListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsInventoryLocationLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByInventoryLocationCodeAsync(
        string inventoryLocationCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
