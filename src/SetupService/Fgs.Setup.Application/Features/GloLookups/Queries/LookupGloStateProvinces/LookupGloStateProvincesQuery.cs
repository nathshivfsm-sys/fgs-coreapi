using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloStateProvinces;

public sealed record LookupGloStateProvincesQuery(string CountryCode, bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloStateProvinceLookupDto>>>;
