using Fgs.Foundation.Paging;
using Fgs.Scheduling.Application.Common.SchedulingCrud;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;

namespace Fgs.Scheduling.Application.Abstractions.Appointments;

public interface IFgsAppointmentReadRepository
{
    Task<FgsAppointmentDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsAppointmentSummaryDto>> ListAsync(
        SchedulingListQuery query,
        FgsAppointmentListFilters filters,
        CancellationToken cancellationToken = default);
}
