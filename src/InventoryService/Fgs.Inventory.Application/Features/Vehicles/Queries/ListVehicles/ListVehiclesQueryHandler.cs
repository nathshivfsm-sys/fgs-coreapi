using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.Vehicles;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vehicles.Queries.ListVehicles;

public sealed class ListVehiclesQueryHandler(IFgsVehicleReadRepository readRepository)
    : IRequestHandler<ListVehiclesQuery, ApiResponse<PagedResult<FgsVehicleSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsVehicleSummaryDto>>> Handle(
        ListVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsVehicleSummaryDto>>.Ok(result);
    }
}
