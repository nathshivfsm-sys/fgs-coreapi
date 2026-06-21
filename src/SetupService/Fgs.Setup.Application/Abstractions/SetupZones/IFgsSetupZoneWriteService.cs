using Fgs.Setup.Application.Features.SetupZones.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupZones;

public interface IFgsSetupZoneWriteService
{
    Task<FgsSetupZoneDetailDto> CreateAsync(FgsSetupZoneCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupZoneDetailDto> UpdateAsync(long id, FgsSetupZoneUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupZoneDetailDto> PatchAsync(long id, FgsSetupZonePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupZoneDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
