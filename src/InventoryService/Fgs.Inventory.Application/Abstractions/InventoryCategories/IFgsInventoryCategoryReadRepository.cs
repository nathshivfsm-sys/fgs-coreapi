using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryCategories;

public interface IFgsInventoryCategoryReadRepository
{
    Task<FgsInventoryCategoryDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsInventoryCategorySummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryCategoryListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsInventoryCategoryLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCategoryCodeAsync(
        string categoryCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default);
}
