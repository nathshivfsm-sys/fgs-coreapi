using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Commands.PatchFgsSetupZone;

public sealed record PatchFgsSetupZoneCommand(long Id, FgsSetupZonePatchDto Dto)
    : IRequest<ApiResponse<FgsSetupZoneDetailDto>>;
