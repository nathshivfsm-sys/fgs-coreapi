using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.ListUniversalMatrixSizeTiers;

public sealed class ListUniversalMatrixSizeTiersQueryHandler(IFgsUniversalMatrixSizeTierReadRepository readRepository)
    : IRequestHandler<ListUniversalMatrixSizeTiersQuery, ApiResponse<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>>> Handle(
        ListUniversalMatrixSizeTiersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>>.Ok(result);
    }
}
