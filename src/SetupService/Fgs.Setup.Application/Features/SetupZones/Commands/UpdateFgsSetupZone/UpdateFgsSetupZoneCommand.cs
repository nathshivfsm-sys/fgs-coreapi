using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Commands.UpdateFgsSetupZone;

public sealed record UpdateFgsSetupZoneCommand(long Id, FgsSetupZoneUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupZoneDetailDto>>;
