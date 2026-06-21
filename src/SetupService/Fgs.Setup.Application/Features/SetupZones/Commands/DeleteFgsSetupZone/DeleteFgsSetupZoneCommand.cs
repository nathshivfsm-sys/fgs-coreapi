using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Commands.DeleteFgsSetupZone;

public sealed record DeleteFgsSetupZoneCommand(long Id)
    : IRequest<ApiResponse<FgsSetupZoneDetailDto>>;
