using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Queries.ListFgsEntityDefaultTermsConditions;

public sealed class ListFgsEntityDefaultTermsConditionsQueryHandler(
    IFgsEntityDefaultTermsConditionReadRepository readRepository)
    : IRequestHandler<ListFgsEntityDefaultTermsConditionsQuery, ApiResponse<PagedResult<FgsEntityDefaultTermsConditionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsEntityDefaultTermsConditionSummaryDto>>> Handle(
        ListFgsEntityDefaultTermsConditionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsEntityDefaultTermsConditionSummaryDto>>.Ok(result);
    }
}
