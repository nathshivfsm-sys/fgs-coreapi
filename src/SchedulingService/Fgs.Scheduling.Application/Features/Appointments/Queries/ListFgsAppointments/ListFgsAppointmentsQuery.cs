using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Scheduling.Application.Common.SchedulingCrud;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;
using MediatR;

namespace Fgs.Scheduling.Application.Features.Appointments.Queries.ListFgsAppointments;

public sealed record ListFgsAppointmentsQuery(
    SchedulingListQuery Query,
    FgsAppointmentListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAppointmentSummaryDto>>>;
