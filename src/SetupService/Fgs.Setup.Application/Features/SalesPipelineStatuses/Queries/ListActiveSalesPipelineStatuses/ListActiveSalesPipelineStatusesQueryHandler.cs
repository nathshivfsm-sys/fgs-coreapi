using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.ListActiveSalesPipelineStatuses;

public sealed class ListActiveSalesPipelineStatusesQueryHandler(IFgsSalesPipelineStatusReadRepository readRepository)
    : IRequestHandler<ListActiveSalesPipelineStatusesQuery, ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>> Handle(
        ListActiveSalesPipelineStatusesQuery request,
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
                request.Filters ?? new FgsSalesPipelineStatusListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSalesPipelineStatusSummaryDto>>(ex);
        }
    }
}
