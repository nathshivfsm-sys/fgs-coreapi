using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Commands.CreateFgsSetupZone;

public sealed record CreateFgsSetupZoneCommand(FgsSetupZoneCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupZoneDetailDto>>;
