using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
namespace Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
public interface IFgsAssetAttributeOptionWriteService {
    Task<FgsAssetAttributeOptionDetailDto> CreateAsync(FgsAssetAttributeOptionCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetAttributeOptionDetailDto> UpdateAsync(long id, FgsAssetAttributeOptionUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetAttributeOptionDetailDto> PatchAsync(long id, FgsAssetAttributeOptionPatchDto dto, CancellationToken cancellationToken = default);
}
