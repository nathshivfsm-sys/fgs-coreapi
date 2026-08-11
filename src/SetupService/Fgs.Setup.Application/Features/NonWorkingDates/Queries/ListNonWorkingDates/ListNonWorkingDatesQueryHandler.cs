using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.NonWorkingDates;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Queries.ListNonWorkingDates;

public sealed class ListNonWorkingDatesQueryHandler(IFgsNonWorkingDateReadRepository readRepository)
    : IRequestHandler<ListNonWorkingDatesQuery, ApiResponse<PagedResult<FgsNonWorkingDateSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsNonWorkingDateSummaryDto>>> Handle(
        ListNonWorkingDatesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsNonWorkingDateSummaryDto>>.Ok(result);
    }
}
