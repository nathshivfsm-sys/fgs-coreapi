using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Queries.ListUniversalPricingServices;

public sealed class ListUniversalPricingServicesQueryHandler(IFgsUniversalPricingServiceReadRepository readRepository)
    : IRequestHandler<ListUniversalPricingServicesQuery, ApiResponse<PagedResult<FgsUniversalPricingServiceSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUniversalPricingServiceSummaryDto>>> Handle(
        ListUniversalPricingServicesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsUniversalPricingServiceSummaryDto>>.Ok(result);
    }
}
