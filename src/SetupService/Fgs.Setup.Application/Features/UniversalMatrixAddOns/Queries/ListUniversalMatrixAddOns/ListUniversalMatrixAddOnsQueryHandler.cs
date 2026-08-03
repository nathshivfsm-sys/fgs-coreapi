using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.ListUniversalMatrixAddOns;

public sealed class ListUniversalMatrixAddOnsQueryHandler(IFgsUniversalMatrixAddOnReadRepository readRepository)
    : IRequestHandler<ListUniversalMatrixAddOnsQuery, ApiResponse<PagedResult<FgsUniversalMatrixAddOnSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalMatrixAddOnSummaryDto>>> Handle(
        ListUniversalMatrixAddOnsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsUniversalMatrixAddOnSummaryDto>>.Ok(result);
    }
}
