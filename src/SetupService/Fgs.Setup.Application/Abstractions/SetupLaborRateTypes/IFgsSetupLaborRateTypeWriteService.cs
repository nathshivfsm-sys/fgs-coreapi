using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;

public interface IFgsSetupLaborRateTypeWriteService
{
    Task<FgsSetupLaborRateTypeDetailDto> CreateAsync(FgsSetupLaborRateTypeCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupLaborRateTypeDetailDto> UpdateAsync(long id, FgsSetupLaborRateTypeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupLaborRateTypeDetailDto> PatchAsync(long id, FgsSetupLaborRateTypePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupLaborRateTypeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
