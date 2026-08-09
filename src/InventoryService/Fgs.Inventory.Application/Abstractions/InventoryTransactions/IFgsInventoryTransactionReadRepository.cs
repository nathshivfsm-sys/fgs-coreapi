using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryTransactions;

public interface IFgsInventoryTransactionReadRepository
{
    Task<FgsInventoryTransactionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsInventoryTransactionSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryTransactionListFilters filters,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByTransactionNumberAsync(
        string transactionNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default);
}
