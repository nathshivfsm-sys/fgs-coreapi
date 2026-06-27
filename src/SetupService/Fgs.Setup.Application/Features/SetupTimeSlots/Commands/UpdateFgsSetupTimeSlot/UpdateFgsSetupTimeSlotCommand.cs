using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Commands.UpdateFgsSetupTimeSlot;

public sealed record UpdateFgsSetupTimeSlotCommand(long Id, FgsSetupTimeSlotUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupTimeSlotDetailDto>>;
