using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupDescriptions;

public interface IFgsSetupDescriptionWriteService
{
    Task<FgsSetupDescriptionDetailDto> CreateAsync(FgsSetupDescriptionCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupDescriptionDetailDto> UpdateAsync(long id, FgsSetupDescriptionUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupDescriptionDetailDto> PatchAsync(long id, FgsSetupDescriptionPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupDescriptionDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
