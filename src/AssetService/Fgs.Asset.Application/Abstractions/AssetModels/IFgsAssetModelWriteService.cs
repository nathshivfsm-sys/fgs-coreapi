using Fgs.Asset.Application.Features.AssetModels.Dtos;
namespace Fgs.Asset.Application.Abstractions.AssetModels;
public interface IFgsAssetModelWriteService
{
    Task<FgsAssetModelDetailDto> CreateAsync(FgsAssetModelCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetModelDetailDto> UpdateAsync(long id, FgsAssetModelUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetModelDetailDto> PatchAsync(long id, FgsAssetModelPatchDto dto, CancellationToken cancellationToken = default);
}
