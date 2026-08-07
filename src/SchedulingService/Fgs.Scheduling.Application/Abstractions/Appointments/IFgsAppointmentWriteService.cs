using Fgs.Scheduling.Application.Features.Appointments.Dtos;

namespace Fgs.Scheduling.Application.Abstractions.Appointments;

public interface IFgsAppointmentWriteService
{
    Task<FgsAppointmentDetailDto> CreateAsync(
        FgsAppointmentCreateDto dto,
        CancellationToken cancellationToken = default);
}
