using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Queries.LookupUniversalMatrixItems;

public sealed record LookupUniversalMatrixItemsQuery(bool ActiveOnly = true, long? UniversalPricingServiceId = null)
    : IRequest<ApiResponse<IReadOnlyList<FgsUniversalMatrixItemLookupDto>>>;
