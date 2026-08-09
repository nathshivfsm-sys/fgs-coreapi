using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.VendorInventoryItems;

public interface IFgsVendorInventoryItemWriteService
{
    Task<FgsVendorInventoryItemDetailDto> CreateAsync(
        FgsVendorInventoryItemCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsVendorInventoryItemDetailDto> UpdateAsync(
        long id,
        FgsVendorInventoryItemUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsVendorInventoryItemDetailDto> PatchAsync(
        long id,
        FgsVendorInventoryItemPatchDto dto,
        CancellationToken cancellationToken = default);
}
