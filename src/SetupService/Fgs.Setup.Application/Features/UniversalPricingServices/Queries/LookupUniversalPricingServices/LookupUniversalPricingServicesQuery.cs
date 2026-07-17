using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Queries.LookupUniversalPricingServices;

public sealed record LookupUniversalPricingServicesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsUniversalPricingServiceLookupDto>>>;
