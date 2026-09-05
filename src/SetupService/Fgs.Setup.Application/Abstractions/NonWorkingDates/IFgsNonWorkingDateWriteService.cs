using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;

namespace Fgs.Setup.Application.Abstractions.NonWorkingDates;

public interface IFgsNonWorkingDateWriteService
{
    Task<FgsNonWorkingDateDetailDto> CreateAsync(
        FgsNonWorkingDateCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsNonWorkingDateDetailDto> UpdateAsync(
        long id,
        FgsNonWorkingDateUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsNonWorkingDateDetailDto> PatchAsync(
        long id,
        FgsNonWorkingDatePatchDto dto,
        CancellationToken cancellationToken = default);
}
