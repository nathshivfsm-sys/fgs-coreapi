using Fgs.Setup.Application.Features.SetupTaxes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTaxes;

public interface IFgsSetupTaxWriteService
{
    Task<FgsSetupTaxDetailDto> CreateAsync(FgsSetupTaxCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTaxDetailDto> UpdateAsync(long id, FgsSetupTaxUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTaxDetailDto> PatchAsync(long id, FgsSetupTaxPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTaxDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
