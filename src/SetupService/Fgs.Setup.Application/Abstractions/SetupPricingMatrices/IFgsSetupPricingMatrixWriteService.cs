using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrices;

public interface IFgsSetupPricingMatrixWriteService
{
    Task<FgsSetupPricingMatrixDetailDto> CreateAsync(
        FgsSetupPricingMatrixCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsSetupPricingMatrixDetailDto> UpdateAsync(
        long id,
        FgsSetupPricingMatrixUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsSetupPricingMatrixDetailDto> PatchAsync(
        long id,
        FgsSetupPricingMatrixPatchDto dto,
        CancellationToken cancellationToken = default);
}
