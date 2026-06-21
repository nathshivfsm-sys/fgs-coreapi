using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Commands.CreateFgsSetupTimeSlot;

public sealed record CreateFgsSetupTimeSlotCommand(FgsSetupTimeSlotCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupTimeSlotDetailDto>>;
