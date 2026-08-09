using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.ListFgsSetupPricingMatrixMaterialTiers;

public sealed class ListFgsSetupPricingMatrixMaterialTiersQueryHandler(IFgsSetupPricingMatrixMaterialTierReadRepository readRepository) : IRequestHandler<ListFgsSetupPricingMatrixMaterialTiersQuery, ApiResponse<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>>> Handle(ListFgsSetupPricingMatrixMaterialTiersQuery request, CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>>.Ok(result);
    }
}
