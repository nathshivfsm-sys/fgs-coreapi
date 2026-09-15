using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.LookupPostalCodeCities;

public sealed record LookupPostalCodeCitiesQuery(
    string? CountryCode = null,
    string? StateProvinceCode = null,
    bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<PostalCodeCityLookupDto>>>;
