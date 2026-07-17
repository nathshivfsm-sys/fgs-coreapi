using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixItems;

public interface IFgsUniversalMatrixItemWriteRepository
{
    Task<FgsUniversalMatrixItemDetailDto> CreateAsync(FgsUniversalMatrixItemCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixItemDetailDto> UpdateAsync(long id, FgsUniversalMatrixItemUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixItemDetailDto> PatchAsync(long id, FgsUniversalMatrixItemPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixItemDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
