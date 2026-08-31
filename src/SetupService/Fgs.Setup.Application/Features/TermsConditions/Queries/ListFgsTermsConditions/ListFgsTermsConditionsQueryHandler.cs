using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.TermsConditions;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TermsConditions.Queries.ListFgsTermsConditions;

public sealed class ListFgsTermsConditionsQueryHandler(IFgsTermsConditionReadRepository readRepository)
    : IRequestHandler<ListFgsTermsConditionsQuery, ApiResponse<PagedResult<FgsTermsConditionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsTermsConditionSummaryDto>>> Handle(
        ListFgsTermsConditionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsTermsConditionSummaryDto>>.Ok(result);
    }
}
