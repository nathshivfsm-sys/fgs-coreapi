using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
namespace Fgs.Asset.Application.Abstractions.AssetWarranties;
public interface IFgsAssetWarrantyWriteService {
    Task<FgsAssetWarrantyDetailDto> CreateAsync(FgsAssetWarrantyCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetWarrantyDetailDto> UpdateAsync(long id, FgsAssetWarrantyUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetWarrantyDetailDto> PatchAsync(long id, FgsAssetWarrantyPatchDto dto, CancellationToken cancellationToken = default);
}
