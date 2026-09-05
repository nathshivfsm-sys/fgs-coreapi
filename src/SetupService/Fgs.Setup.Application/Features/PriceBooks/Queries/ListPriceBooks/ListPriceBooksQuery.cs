using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBooks.Queries.ListPriceBooks;

public sealed record ListPriceBooksQuery(
    SetupListQuery Query,
    FgsPriceBookListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsPriceBookSummaryDto>>>;
