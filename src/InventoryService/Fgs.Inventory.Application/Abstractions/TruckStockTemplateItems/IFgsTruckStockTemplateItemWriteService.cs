using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;

public interface IFgsTruckStockTemplateItemWriteService
{
    Task<FgsTruckStockTemplateItemDetailDto> CreateAsync(
        long templateId,
        FgsTruckStockTemplateItemCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsTruckStockTemplateItemDetailDto> UpdateAsync(
        long templateId,
        long itemId,
        FgsTruckStockTemplateItemUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsTruckStockTemplateItemDetailDto> PatchAsync(
        long templateId,
        long itemId,
        FgsTruckStockTemplateItemPatchDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(long templateId, long itemId, CancellationToken cancellationToken = default);
}
