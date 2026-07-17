using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiEvents.Queries.ListFgsApiEvents;

public sealed class ListFgsApiEventsQueryHandler(IFgsApiEventReadRepository readRepository)
    : IRequestHandler<ListFgsApiEventsQuery, ApiResponse<PagedResult<FgsApiEventSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsApiEventSummaryDto>>> Handle(
        ListFgsApiEventsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsApiEventSummaryDto>>.Ok(result);
    }
}
