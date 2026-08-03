using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;

public interface IFgsSetupPricingMatrixLaborTierWriteService
{
    Task<FgsSetupPricingMatrixLaborTierDetailDto> CreateAsync(FgsSetupPricingMatrixLaborTierCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixLaborTierDetailDto> UpdateAsync(long id, FgsSetupPricingMatrixLaborTierUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixLaborTierDetailDto> PatchAsync(long id, FgsSetupPricingMatrixLaborTierPatchDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixLaborTierDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
