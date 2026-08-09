using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.ListFgsSetupPricingMatrixOthers;

public sealed class ListFgsSetupPricingMatrixOthersQueryHandler(IFgsSetupPricingMatrixOtherReadRepository readRepository) : IRequestHandler<ListFgsSetupPricingMatrixOthersQuery, ApiResponse<PagedResult<FgsSetupPricingMatrixOtherSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPricingMatrixOtherSummaryDto>>> Handle(ListFgsSetupPricingMatrixOthersQuery request, CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPricingMatrixOtherSummaryDto>>.Ok(result);
    }
}
