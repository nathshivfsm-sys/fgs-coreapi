using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Queries.GetFgsSetupTimeSlotById;

public sealed record GetFgsSetupTimeSlotByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupTimeSlotDetailDto>>;
