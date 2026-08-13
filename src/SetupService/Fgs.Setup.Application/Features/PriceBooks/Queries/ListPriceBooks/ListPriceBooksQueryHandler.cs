using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.PriceBooks;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBooks.Queries.ListPriceBooks;

public sealed class ListPriceBooksQueryHandler(IFgsPriceBookReadRepository readRepository)
    : IRequestHandler<ListPriceBooksQuery, ApiResponse<PagedResult<FgsPriceBookSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsPriceBookSummaryDto>>> Handle(
        ListPriceBooksQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsPriceBookSummaryDto>>.Ok(result);
    }
}
