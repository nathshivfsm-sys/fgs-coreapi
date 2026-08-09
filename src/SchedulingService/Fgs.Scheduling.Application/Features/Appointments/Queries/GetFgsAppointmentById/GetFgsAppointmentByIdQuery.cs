using Fgs.Contracts.Api;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;
using MediatR;

namespace Fgs.Scheduling.Application.Features.Appointments.Queries.GetFgsAppointmentById;

public sealed record GetFgsAppointmentByIdQuery(long Id)
    : IRequest<ApiResponse<FgsAppointmentDetailDto>>;
