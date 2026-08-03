using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.LookupUniversalMatrixAddOns;

public sealed record LookupUniversalMatrixAddOnsQuery(bool ActiveOnly = true, long? UniversalPricingServiceId = null)
    : IRequest<ApiResponse<IReadOnlyList<FgsUniversalMatrixAddOnLookupDto>>>;
