using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.ListFgsSetupPricingMatrixLaborTiers;

public sealed class ListFgsSetupPricingMatrixLaborTiersQueryHandler(IFgsSetupPricingMatrixLaborTierReadRepository readRepository) : IRequestHandler<ListFgsSetupPricingMatrixLaborTiersQuery, ApiResponse<PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>>> Handle(ListFgsSetupPricingMatrixLaborTiersQuery request, CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>>.Ok(result);
    }
}
