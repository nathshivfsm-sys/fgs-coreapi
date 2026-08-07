using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Scheduling.Application.Abstractions.Appointments;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;
using MediatR;

namespace Fgs.Scheduling.Application.Features.Appointments.Queries.ListFgsAppointments;

public sealed class ListFgsAppointmentsQueryHandler(IFgsAppointmentReadRepository readRepository)
    : IRequestHandler<ListFgsAppointmentsQuery, ApiResponse<PagedResult<FgsAppointmentSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAppointmentSummaryDto>>> Handle(
        ListFgsAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAppointmentSummaryDto>>.Ok(result);
    }
}
