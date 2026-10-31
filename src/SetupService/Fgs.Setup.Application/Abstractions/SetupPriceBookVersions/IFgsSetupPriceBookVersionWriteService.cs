using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;

public interface IFgsSetupPriceBookVersionWriteService
{
    Task<FgsSetupPriceBookVersionDetailDto> CreateAsync(
        FgsSetupPriceBookVersionCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsSetupPriceBookVersionDetailDto> UpdateAsync(
        long id,
        FgsSetupPriceBookVersionUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsSetupPriceBookVersionDetailDto> PatchAsync(
        long id,
        FgsSetupPriceBookVersionPatchDto dto,
        CancellationToken cancellationToken = default);
}
