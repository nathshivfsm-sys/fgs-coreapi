using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;

public interface IFgsUniversalMatrixTierWriteService
{
    Task<FgsUniversalMatrixTierDetailDto> CreateAsync(FgsUniversalMatrixTierCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixTierDetailDto> UpdateAsync(long id, FgsUniversalMatrixTierUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixTierDetailDto> PatchAsync(long id, FgsUniversalMatrixTierPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixTierDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
