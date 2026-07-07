using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Queries.ListUniversalMatrixOneTimeFees;

public sealed class ListUniversalMatrixOneTimeFeesQueryHandler(IFgsUniversalMatrixOneTimeFeeReadRepository readRepository)
    : IRequestHandler<ListUniversalMatrixOneTimeFeesQuery, ApiResponse<PagedResult<FgsUniversalMatrixOneTimeFeeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalMatrixOneTimeFeeSummaryDto>>> Handle(
        ListUniversalMatrixOneTimeFeesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsUniversalMatrixOneTimeFeeSummaryDto>>.Ok(result);
    }
}
