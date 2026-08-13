using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Queries.ListPriceBookItems;

public sealed record ListPriceBookItemsQuery(
    SetupListQuery Query,
    FgsPriceBookItemListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsPriceBookItemSummaryDto>>>;
