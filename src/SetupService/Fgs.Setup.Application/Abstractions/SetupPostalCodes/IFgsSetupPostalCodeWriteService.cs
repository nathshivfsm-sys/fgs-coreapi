using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPostalCodes;

public interface IFgsSetupPostalCodeWriteService
{
    Task<FgsSetupPostalCodeDetailDto> CreateAsync(FgsSetupPostalCodeCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupPostalCodeDetailDto> UpdateAsync(long id, FgsSetupPostalCodeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupPostalCodeDetailDto> PatchAsync(long id, FgsSetupPostalCodePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupPostalCodeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
