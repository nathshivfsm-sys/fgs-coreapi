using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.ListUniversalMatrixFrequencyDiscounts;

public sealed record ListUniversalMatrixFrequencyDiscountsQuery(
    SetupListQuery Query, FgsUniversalMatrixFrequencyDiscountListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>>>;
