using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.LookupUniversalMatrixSizeTiers;

public sealed record LookupUniversalMatrixSizeTiersQuery(bool ActiveOnly = true, long? UniversalPricingServiceId = null)
    : IRequest<ApiResponse<IReadOnlyList<FgsUniversalMatrixSizeTierLookupDto>>>;
