using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;

namespace Fgs.Inventory.Application.Abstractions.VehicleMaintenances;

public interface IFgsVehicleMaintenanceReadRepository
{
    Task<FgsVehicleMaintenanceDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsVehicleMaintenanceSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsVehicleMaintenanceListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsVehicleMaintenanceLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsVehicleIdAsync(
        long id,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsGloVehicleMaintenanceTypeIdAsync(
        int id,
        CancellationToken cancellationToken = default);
}
