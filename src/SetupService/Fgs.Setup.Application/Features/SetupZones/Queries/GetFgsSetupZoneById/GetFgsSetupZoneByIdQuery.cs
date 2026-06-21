using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.GetFgsSetupZoneById;

public sealed record GetFgsSetupZoneByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupZoneDetailDto>>;
