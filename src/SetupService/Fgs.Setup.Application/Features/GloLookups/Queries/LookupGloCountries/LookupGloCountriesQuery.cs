using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloCountries;

public sealed record LookupGloCountriesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloCountryLookupDto>>>;
