using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.ListActiveUniversalMatrixFrequencyDiscounts;

public sealed record ListActiveUniversalMatrixFrequencyDiscountsQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsUniversalMatrixFrequencyDiscountListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>>>;
