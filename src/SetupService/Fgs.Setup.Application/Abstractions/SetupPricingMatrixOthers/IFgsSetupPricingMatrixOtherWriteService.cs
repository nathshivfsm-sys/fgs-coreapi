using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;

public interface IFgsSetupPricingMatrixOtherWriteService
{
    Task<FgsSetupPricingMatrixOtherDetailDto> CreateAsync(FgsSetupPricingMatrixOtherCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixOtherDetailDto> UpdateAsync(long id, FgsSetupPricingMatrixOtherUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixOtherDetailDto> PatchAsync(long id, FgsSetupPricingMatrixOtherPatchDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixOtherDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
