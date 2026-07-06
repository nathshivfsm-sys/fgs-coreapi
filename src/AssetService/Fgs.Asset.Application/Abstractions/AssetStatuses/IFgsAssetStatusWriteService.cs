using Fgs.Asset.Application.Features.AssetStatuses.Dtos;

namespace Fgs.Asset.Application.Abstractions.AssetStatuses;

public interface IFgsAssetStatusWriteService
{
    Task<FgsAssetStatusDetailDto> CreateAsync(FgsAssetStatusCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetStatusDetailDto> UpdateAsync(long id, FgsAssetStatusUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsAssetStatusDetailDto> PatchAsync(long id, FgsAssetStatusPatchDto dto, CancellationToken cancellationToken = default);
}
