using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Queries.ListLeadSources;

public sealed class ListLeadSourcesQueryHandler(ILeadSourceReadRepository readRepository)
    : IRequestHandler<ListLeadSourcesQuery, ApiResponse<PagedResult<LeadSourceSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<LeadSourceSummaryDto>>> Handle(
        ListLeadSourcesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<LeadSourceSummaryDto>>.Ok(result);
    }
}
