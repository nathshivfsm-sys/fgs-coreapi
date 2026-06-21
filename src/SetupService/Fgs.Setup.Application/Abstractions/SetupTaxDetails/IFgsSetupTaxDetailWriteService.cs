using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTaxDetails;

public interface IFgsSetupTaxDetailWriteService
{
    Task<FgsSetupTaxDetailDetailDto> CreateAsync(FgsSetupTaxDetailCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTaxDetailDetailDto> UpdateAsync(long id, FgsSetupTaxDetailUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTaxDetailDetailDto> PatchAsync(long id, FgsSetupTaxDetailPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTaxDetailDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
