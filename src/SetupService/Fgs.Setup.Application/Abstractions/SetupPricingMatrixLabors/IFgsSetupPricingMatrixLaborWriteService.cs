using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;

public interface IFgsSetupPricingMatrixLaborWriteService
{
    Task<FgsSetupPricingMatrixLaborDetailDto> CreateAsync(FgsSetupPricingMatrixLaborCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixLaborDetailDto> UpdateAsync(long id, FgsSetupPricingMatrixLaborUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixLaborDetailDto> PatchAsync(long id, FgsSetupPricingMatrixLaborPatchDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixLaborDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
