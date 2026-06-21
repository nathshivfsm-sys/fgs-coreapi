using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Queries.ListActiveWarehouses;

public sealed class ListActiveWarehousesQueryHandler(IFgsWarehouseReadRepository readRepository)
    : IRequestHandler<ListActiveWarehousesQuery, ApiResponse<PagedResult<FgsWarehouseSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsWarehouseSummaryDto>>> Handle(
        ListActiveWarehousesQuery request,
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
                request.Filters ?? new FgsWarehouseListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsWarehouseSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsWarehouseSummaryDto>>(ex);
        }
    }
}
