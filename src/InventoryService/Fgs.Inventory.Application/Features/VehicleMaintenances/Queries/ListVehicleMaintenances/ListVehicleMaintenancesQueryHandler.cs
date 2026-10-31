using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.VehicleMaintenances;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VehicleMaintenances.Queries.ListVehicleMaintenances;

public sealed class ListVehicleMaintenancesQueryHandler(IFgsVehicleMaintenanceReadRepository readRepository)
    : IRequestHandler<ListVehicleMaintenancesQuery, ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>> Handle(
        ListVehicleMaintenancesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>.Ok(result);
    }
}
