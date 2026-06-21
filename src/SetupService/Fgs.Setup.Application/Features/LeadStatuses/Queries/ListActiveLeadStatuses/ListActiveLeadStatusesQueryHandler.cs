using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.ListActiveLeadStatuses;

public sealed class ListActiveLeadStatusesQueryHandler(ILeadStatusReadRepository readRepository)
    : IRequestHandler<ListActiveLeadStatusesQuery, ApiResponse<PagedResult<LeadStatusSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<LeadStatusSummaryDto>>> Handle(
        ListActiveLeadStatusesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                query,
                request.Filters ?? new LeadStatusListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<LeadStatusSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<LeadStatusSummaryDto>>(ex);
        }
    }
}
