using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Commands.DeleteFgsSetupTimeSlot;

public sealed record DeleteFgsSetupTimeSlotCommand(long Id)
    : IRequest<ApiResponse<FgsSetupTimeSlotDetailDto>>;
