using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Queries.ListSetupPricingMatrices;

public sealed class ListSetupPricingMatricesQueryHandler(
    IFgsSetupPricingMatrixReadRepository readRepository)
    : IRequestHandler<ListSetupPricingMatricesQuery, ApiResponse<PagedResult<FgsSetupPricingMatrixSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPricingMatrixSummaryDto>>> Handle(
        ListSetupPricingMatricesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPricingMatrixSummaryDto>>.Ok(result);
    }
}
