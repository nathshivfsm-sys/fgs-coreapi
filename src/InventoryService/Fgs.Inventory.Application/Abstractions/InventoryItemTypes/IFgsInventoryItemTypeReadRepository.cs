using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryItemTypes;

public interface IFgsInventoryItemTypeReadRepository
{
    Task<FgsInventoryItemTypeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsInventoryItemTypeSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryItemTypeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsInventoryItemTypeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByItemTypeCodeAsync(
        string itemTypeCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default);
}
