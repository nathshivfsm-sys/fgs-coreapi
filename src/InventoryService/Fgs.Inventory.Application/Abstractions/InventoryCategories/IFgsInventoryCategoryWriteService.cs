using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryCategories;

public interface IFgsInventoryCategoryWriteService
{
    Task<FgsInventoryCategoryDetailDto> CreateAsync(FgsInventoryCategoryCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInventoryCategoryDetailDto> UpdateAsync(long id, FgsInventoryCategoryUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInventoryCategoryDetailDto> PatchAsync(long id, FgsInventoryCategoryPatchDto dto, CancellationToken cancellationToken = default);
}
