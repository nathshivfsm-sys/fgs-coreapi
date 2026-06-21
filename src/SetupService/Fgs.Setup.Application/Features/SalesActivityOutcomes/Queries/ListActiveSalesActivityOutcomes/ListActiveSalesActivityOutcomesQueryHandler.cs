using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.ListActiveSalesActivityOutcomes;

public sealed class ListActiveSalesActivityOutcomesQueryHandler(IFgsSalesActivityOutcomeReadRepository readRepository)
    : IRequestHandler<ListActiveSalesActivityOutcomesQuery, ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>> Handle(
        ListActiveSalesActivityOutcomesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                query,
                request.Filters ?? new FgsSalesActivityOutcomeListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSalesActivityOutcomeSummaryDto>>(ex);
        }
    }
}
