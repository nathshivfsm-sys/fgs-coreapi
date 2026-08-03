using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Queries.ListUniversalMatrixItems;

public sealed class ListUniversalMatrixItemsQueryHandler(IFgsUniversalMatrixItemReadRepository readRepository)
    : IRequestHandler<ListUniversalMatrixItemsQuery, ApiResponse<PagedResult<FgsUniversalMatrixItemSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalMatrixItemSummaryDto>>> Handle(
        ListUniversalMatrixItemsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsUniversalMatrixItemSummaryDto>>.Ok(result);
    }
}
