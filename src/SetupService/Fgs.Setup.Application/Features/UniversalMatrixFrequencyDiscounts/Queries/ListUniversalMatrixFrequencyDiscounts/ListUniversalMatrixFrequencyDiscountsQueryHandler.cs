using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.ListUniversalMatrixFrequencyDiscounts;

public sealed class ListUniversalMatrixFrequencyDiscountsQueryHandler(IFgsUniversalMatrixFrequencyDiscountReadRepository readRepository)
    : IRequestHandler<ListUniversalMatrixFrequencyDiscountsQuery, ApiResponse<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>>> Handle(
        ListUniversalMatrixFrequencyDiscountsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>>.Ok(result);
    }
}
