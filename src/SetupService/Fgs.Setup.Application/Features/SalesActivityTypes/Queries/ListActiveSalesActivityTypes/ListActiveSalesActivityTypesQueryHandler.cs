using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.ListActiveSalesActivityTypes;

public sealed class ListActiveSalesActivityTypesQueryHandler(IFgsSalesActivityTypeReadRepository readRepository)
    : IRequestHandler<ListActiveSalesActivityTypesQuery, ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>> Handle(
        ListActiveSalesActivityTypesQuery request,
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
                request.Filters ?? new FgsSalesActivityTypeListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSalesActivityTypeSummaryDto>>(ex);
        }
    }
}
