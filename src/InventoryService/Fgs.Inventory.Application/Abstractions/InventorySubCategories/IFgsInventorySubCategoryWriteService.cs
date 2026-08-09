using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventorySubCategories;

public interface IFgsInventorySubCategoryWriteService
{
    Task<FgsInventorySubCategoryDetailDto> CreateAsync(FgsInventorySubCategoryCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInventorySubCategoryDetailDto> UpdateAsync(long id, FgsInventorySubCategoryUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInventorySubCategoryDetailDto> PatchAsync(long id, FgsInventorySubCategoryPatchDto dto, CancellationToken cancellationToken = default);
}
