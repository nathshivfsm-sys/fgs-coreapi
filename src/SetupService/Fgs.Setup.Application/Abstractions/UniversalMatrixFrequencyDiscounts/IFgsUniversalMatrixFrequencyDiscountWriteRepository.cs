using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;

public interface IFgsUniversalMatrixFrequencyDiscountWriteRepository
{
    Task<FgsUniversalMatrixFrequencyDiscountDetailDto> CreateAsync(FgsUniversalMatrixFrequencyDiscountCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixFrequencyDiscountDetailDto> UpdateAsync(long id, FgsUniversalMatrixFrequencyDiscountUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixFrequencyDiscountDetailDto> PatchAsync(long id, FgsUniversalMatrixFrequencyDiscountPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixFrequencyDiscountDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
