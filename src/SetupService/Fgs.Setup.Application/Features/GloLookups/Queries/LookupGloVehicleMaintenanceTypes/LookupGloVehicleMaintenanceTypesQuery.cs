using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloVehicleMaintenanceTypes;

public sealed record LookupGloVehicleMaintenanceTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloVehicleMaintenanceTypeLookupDto>>>;
