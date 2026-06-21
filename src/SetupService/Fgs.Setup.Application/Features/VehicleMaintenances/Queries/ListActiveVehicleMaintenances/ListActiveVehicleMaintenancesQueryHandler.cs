using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Queries.ListActiveVehicleMaintenances;

public sealed class ListActiveVehicleMaintenancesQueryHandler(IFgsVehicleMaintenanceReadRepository readRepository)
    : IRequestHandler<ListActiveVehicleMaintenancesQuery, ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>> Handle(
        ListActiveVehicleMaintenancesQuery request,
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
                request.Filters ?? new FgsVehicleMaintenanceListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsVehicleMaintenanceSummaryDto>>(ex);
        }
    }
}
