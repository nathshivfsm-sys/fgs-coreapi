using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.LookupSetupZones;

public sealed record LookupSetupZonesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupZoneLookupDto>>>;
