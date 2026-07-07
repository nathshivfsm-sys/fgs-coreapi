using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Queries.ListUniversalMatrixTiers;

public sealed class ListUniversalMatrixTiersQueryHandler(IFgsUniversalMatrixTierReadRepository readRepository)
    : IRequestHandler<ListUniversalMatrixTiersQuery, ApiResponse<PagedResult<FgsUniversalMatrixTierSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalMatrixTierSummaryDto>>> Handle(
        ListUniversalMatrixTiersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsUniversalMatrixTierSummaryDto>>.Ok(result);
    }
}
