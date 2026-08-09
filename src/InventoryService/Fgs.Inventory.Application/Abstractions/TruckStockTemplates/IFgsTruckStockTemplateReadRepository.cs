using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;

namespace Fgs.Inventory.Application.Abstractions.TruckStockTemplates;

public interface IFgsTruckStockTemplateReadRepository
{
    Task<FgsTruckStockTemplateDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsTruckStockTemplateSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsTruckStockTemplateListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsTruckStockTemplateLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByTemplateCodeAsync(
        string templateCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default);
}
