using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;

public interface IFgsSetupPricingMatrixMaterialTierWriteService
{
    Task<FgsSetupPricingMatrixMaterialTierDetailDto> CreateAsync(FgsSetupPricingMatrixMaterialTierCreateDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixMaterialTierDetailDto> UpdateAsync(long id, FgsSetupPricingMatrixMaterialTierUpdateDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixMaterialTierDetailDto> PatchAsync(long id, FgsSetupPricingMatrixMaterialTierPatchDto dto, CancellationToken cancellationToken = default);
    Task<FgsSetupPricingMatrixMaterialTierDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
