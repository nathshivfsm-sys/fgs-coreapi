using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.ListSalesDispositionReasons;

public sealed class ListSalesDispositionReasonsQueryHandler(IFgsSalesDispositionReasonReadRepository readRepository)
    : IRequestHandler<ListSalesDispositionReasonsQuery, ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>> Handle(
        ListSalesDispositionReasonsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>.Ok(result);
    }
}
