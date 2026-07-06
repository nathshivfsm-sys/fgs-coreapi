using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
namespace Fgs.Asset.Application.Abstractions.AssetAttributes;
public interface IFgsAssetAttributeWriteService {
    Task<FgsAssetAttributeDetailDto> CreateAsync(FgsAssetAttributeCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetAttributeDetailDto> UpdateAsync(long id, FgsAssetAttributeUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetAttributeDetailDto> PatchAsync(long id, FgsAssetAttributePatchDto dto, CancellationToken cancellationToken = default);
}
