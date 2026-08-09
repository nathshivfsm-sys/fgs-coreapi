using Fgs.Contracts.Api;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;
using MediatR;

namespace Fgs.Scheduling.Application.Features.Appointments.Commands.CreateFgsAppointment;

public sealed record CreateFgsAppointmentCommand(FgsAppointmentCreateDto Dto)
    : IRequest<ApiResponse<FgsAppointmentDetailDto>>;
