using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Queries.ListActiveLeadSources;

public sealed class ListActiveLeadSourcesQueryHandler(ILeadSourceReadRepository readRepository)
    : IRequestHandler<ListActiveLeadSourcesQuery, ApiResponse<PagedResult<LeadSourceSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<LeadSourceSummaryDto>>> Handle(
        ListActiveLeadSourcesQuery request,
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
                request.Filters ?? new LeadSourceListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<LeadSourceSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<LeadSourceSummaryDto>>(ex);
        }
    }
}
