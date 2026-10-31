using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;

namespace Fgs.Inventory.Application.Abstractions.Vehicles;

public interface IFgsVehicleReadRepository
{
    Task<FgsVehicleDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsVehicleSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsVehicleListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsVehicleLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);
}
