using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryStocks;

public interface IFgsInventoryStockWriteService
{
    Task<FgsInventoryStockDetailDto> CreateAsync(
        FgsInventoryStockCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsInventoryStockDetailDto> UpdateAsync(
        long id,
        FgsInventoryStockUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsInventoryStockDetailDto> PatchAsync(
        long id,
        FgsInventoryStockPatchDto dto,
        CancellationToken cancellationToken = default);
}
