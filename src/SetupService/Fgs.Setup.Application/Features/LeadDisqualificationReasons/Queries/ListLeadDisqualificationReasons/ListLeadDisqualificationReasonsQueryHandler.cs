using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.ListLeadDisqualificationReasons;

public sealed class ListLeadDisqualificationReasonsQueryHandler(ILeadDisqualificationReasonReadRepository readRepository)
    : IRequestHandler<ListLeadDisqualificationReasonsQuery, ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>> Handle(
        ListLeadDisqualificationReasonsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<LeadDisqualificationReasonSummaryDto>>(ex);
        }
    }
}
