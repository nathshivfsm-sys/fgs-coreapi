using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.ListSalesActivityOutcomes;

public sealed class ListSalesActivityOutcomesQueryHandler(IFgsSalesActivityOutcomeReadRepository readRepository)
    : IRequestHandler<ListSalesActivityOutcomesQuery, ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>> Handle(
        ListSalesActivityOutcomesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSalesActivityOutcomeSummaryDto>>(ex);
        }
    }
}
