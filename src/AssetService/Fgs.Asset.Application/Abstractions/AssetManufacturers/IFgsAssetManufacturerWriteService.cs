using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;

namespace Fgs.Asset.Application.Abstractions.AssetManufacturers;

public interface IFgsAssetManufacturerWriteService
{
    Task<FgsAssetManufacturerDetailDto> CreateAsync(FgsAssetManufacturerCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetManufacturerDetailDto> UpdateAsync(long id, FgsAssetManufacturerUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetManufacturerDetailDto> PatchAsync(long id, FgsAssetManufacturerPatchDto dto, CancellationToken cancellationToken = default);
}
