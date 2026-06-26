using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.ListLeadStatuses;

public sealed class ListLeadStatusesQueryHandler(ILeadStatusReadRepository readRepository)
    : IRequestHandler<ListLeadStatusesQuery, ApiResponse<PagedResult<LeadStatusSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<LeadStatusSummaryDto>>> Handle(
        ListLeadStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<LeadStatusSummaryDto>>.Ok(result);
    }
}
