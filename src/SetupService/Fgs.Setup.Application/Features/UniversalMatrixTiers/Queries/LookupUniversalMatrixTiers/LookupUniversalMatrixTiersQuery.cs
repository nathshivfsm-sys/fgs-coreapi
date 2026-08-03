using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Queries.LookupUniversalMatrixTiers;

public sealed record LookupUniversalMatrixTiersQuery(bool ActiveOnly = true, long? UniversalPricingServiceId = null)
    : IRequest<ApiResponse<IReadOnlyList<FgsUniversalMatrixTierLookupDto>>>;
