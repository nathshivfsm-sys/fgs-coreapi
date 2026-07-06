using Fgs.Asset.Application.Features.AssetTypes.Dtos;

namespace Fgs.Asset.Application.Abstractions.AssetTypes;

public interface IFgsAssetTypeWriteService
{
    Task<FgsAssetTypeDetailDto> CreateAsync(FgsAssetTypeCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetTypeDetailDto> UpdateAsync(long id, FgsAssetTypeUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetTypeDetailDto> PatchAsync(long id, FgsAssetTypePatchDto dto, CancellationToken cancellationToken = default);
}
