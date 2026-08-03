using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.ListFgsSetupPricingMatrixLabors;

public sealed class ListFgsSetupPricingMatrixLaborsQueryHandler(IFgsSetupPricingMatrixLaborReadRepository readRepository) : IRequestHandler<ListFgsSetupPricingMatrixLaborsQuery, ApiResponse<PagedResult<FgsSetupPricingMatrixLaborSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPricingMatrixLaborSummaryDto>>> Handle(ListFgsSetupPricingMatrixLaborsQuery request, CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPricingMatrixLaborSummaryDto>>.Ok(result);
    }
}
