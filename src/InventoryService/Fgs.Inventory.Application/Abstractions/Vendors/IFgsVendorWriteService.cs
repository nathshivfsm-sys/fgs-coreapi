using Fgs.Inventory.Application.Features.Vendors.Dtos;

namespace Fgs.Inventory.Application.Abstractions.Vendors;

public interface IFgsVendorWriteService
{
    Task<FgsVendorDetailDto> CreateAsync(FgsVendorCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsVendorDetailDto> UpdateAsync(long id, FgsVendorUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsVendorDetailDto> PatchAsync(long id, FgsVendorPatchDto dto, CancellationToken cancellationToken = default);
}
