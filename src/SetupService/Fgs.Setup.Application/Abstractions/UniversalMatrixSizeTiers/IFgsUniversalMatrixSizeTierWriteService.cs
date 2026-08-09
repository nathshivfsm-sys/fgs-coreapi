using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;

public interface IFgsUniversalMatrixSizeTierWriteService
{
    Task<FgsUniversalMatrixSizeTierDetailDto> CreateAsync(FgsUniversalMatrixSizeTierCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixSizeTierDetailDto> UpdateAsync(long id, FgsUniversalMatrixSizeTierUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixSizeTierDetailDto> PatchAsync(long id, FgsUniversalMatrixSizeTierPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixSizeTierDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
