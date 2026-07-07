using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;

public interface IFgsUniversalMatrixOneTimeFeeWriteRepository
{
    Task<FgsUniversalMatrixOneTimeFeeDetailDto> CreateAsync(FgsUniversalMatrixOneTimeFeeCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixOneTimeFeeDetailDto> UpdateAsync(long id, FgsUniversalMatrixOneTimeFeeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixOneTimeFeeDetailDto> PatchAsync(long id, FgsUniversalMatrixOneTimeFeePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixOneTimeFeeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
