using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.ListSalesActivityTypes;

public sealed class ListSalesActivityTypesQueryHandler(IFgsSalesActivityTypeReadRepository readRepository)
    : IRequestHandler<ListSalesActivityTypesQuery, ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>> Handle(
        ListSalesActivityTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>.Ok(result);
    }
}
