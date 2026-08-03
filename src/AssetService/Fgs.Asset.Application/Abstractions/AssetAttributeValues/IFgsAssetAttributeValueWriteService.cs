using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
namespace Fgs.Asset.Application.Abstractions.AssetAttributeValues;
public interface IFgsAssetAttributeValueWriteService {
    Task<FgsAssetAttributeValueDetailDto> CreateAsync(FgsAssetAttributeValueCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetAttributeValueDetailDto> UpdateAsync(long id, FgsAssetAttributeValueUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetAttributeValueDetailDto> PatchAsync(long id, FgsAssetAttributeValuePatchDto dto, CancellationToken cancellationToken = default);
}
