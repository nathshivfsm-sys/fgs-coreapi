using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.ListActiveLeadDisqualificationReasons;

public sealed class ListActiveLeadDisqualificationReasonsQueryHandler(ILeadDisqualificationReasonReadRepository readRepository)
    : IRequestHandler<ListActiveLeadDisqualificationReasonsQuery, ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>> Handle(
        ListActiveLeadDisqualificationReasonsQuery request,
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
                request.Filters ?? new LeadDisqualificationReasonListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<LeadDisqualificationReasonSummaryDto>>(ex);
        }
    }
}
