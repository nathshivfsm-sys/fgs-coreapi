using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;

namespace Fgs.Setup.Application.Abstractions.VehicleMaintenances;

public interface IFgsVehicleMaintenanceReadRepository
{
    Task<FgsVehicleMaintenanceDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsVehicleMaintenanceSummaryDto>> ListAsync(
        SetupListQuery query,
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
