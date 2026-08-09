using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryTransactions;

public interface IFgsInventoryTransactionWriteService
{
    Task<FgsInventoryTransactionDetailDto> CreateAsync(
        FgsInventoryTransactionCreateDto dto,
        CancellationToken cancellationToken = default);
}
