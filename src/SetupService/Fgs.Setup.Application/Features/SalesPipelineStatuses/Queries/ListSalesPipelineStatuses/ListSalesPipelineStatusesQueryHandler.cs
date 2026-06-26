using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.ListSalesPipelineStatuses;

public sealed class ListSalesPipelineStatusesQueryHandler(IFgsSalesPipelineStatusReadRepository readRepository)
    : IRequestHandler<ListSalesPipelineStatusesQuery, ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>> Handle(
        ListSalesPipelineStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>.Ok(result);
    }
}
