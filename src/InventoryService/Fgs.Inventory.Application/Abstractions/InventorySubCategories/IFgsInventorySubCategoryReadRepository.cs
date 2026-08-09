using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventorySubCategories;

public interface IFgsInventorySubCategoryReadRepository
{
    Task<FgsInventorySubCategoryDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsInventorySubCategorySummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventorySubCategoryListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsInventorySubCategoryLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsBySubCategoryCodeAsync(
        long inventoryCategoryId,
        string subCategoryCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default);
}
