using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;

public interface IFgsUniversalMatrixAddOnWriteService
{
    Task<FgsUniversalMatrixAddOnDetailDto> CreateAsync(FgsUniversalMatrixAddOnCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixAddOnDetailDto> UpdateAsync(long id, FgsUniversalMatrixAddOnUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixAddOnDetailDto> PatchAsync(long id, FgsUniversalMatrixAddOnPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalMatrixAddOnDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
