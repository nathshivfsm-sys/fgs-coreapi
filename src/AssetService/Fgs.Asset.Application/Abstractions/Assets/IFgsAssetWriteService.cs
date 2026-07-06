using Fgs.Asset.Application.Features.Assets.Dtos;
namespace Fgs.Asset.Application.Abstractions.Assets;
public interface IFgsAssetWriteService {
    Task<FgsAssetDetailDto> CreateAsync(FgsAssetCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetDetailDto> UpdateAsync(long id, FgsAssetUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetDetailDto> PatchAsync(long id, FgsAssetPatchDto dto, CancellationToken cancellationToken = default);
}
