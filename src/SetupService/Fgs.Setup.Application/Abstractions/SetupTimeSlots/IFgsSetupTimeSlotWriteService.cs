using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTimeSlots;

public interface IFgsSetupTimeSlotWriteService
{
    Task<FgsSetupTimeSlotDetailDto> CreateAsync(FgsSetupTimeSlotCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTimeSlotDetailDto> UpdateAsync(long id, FgsSetupTimeSlotUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTimeSlotDetailDto> PatchAsync(long id, FgsSetupTimeSlotPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTimeSlotDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
