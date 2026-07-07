using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.LookupUniversalMatrixFrequencyDiscounts;

public sealed record LookupUniversalMatrixFrequencyDiscountsQuery(bool ActiveOnly = true, long? UniversalPricingServiceId = null)
    : IRequest<ApiResponse<IReadOnlyList<FgsUniversalMatrixFrequencyDiscountLookupDto>>>;
