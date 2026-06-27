using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Commands.PatchFgsSetupTimeSlot;

public sealed record PatchFgsSetupTimeSlotCommand(long Id, FgsSetupTimeSlotPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupTimeSlotDetailDto>>;
