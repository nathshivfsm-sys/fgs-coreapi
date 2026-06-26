using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Queries.ListWarehouses;

public sealed class ListWarehousesQueryHandler(IFgsWarehouseReadRepository readRepository)
    : IRequestHandler<ListWarehousesQuery, ApiResponse<PagedResult<FgsWarehouseSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsWarehouseSummaryDto>>> Handle(
        ListWarehousesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsWarehouseSummaryDto>>.Ok(result);
    }
}
