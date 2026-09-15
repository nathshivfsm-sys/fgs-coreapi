using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloUnitOfMeasures;

public sealed record LookupGloUnitOfMeasuresQuery(string? UnitType = null, bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloUnitOfMeasureLookupDto>>>;
