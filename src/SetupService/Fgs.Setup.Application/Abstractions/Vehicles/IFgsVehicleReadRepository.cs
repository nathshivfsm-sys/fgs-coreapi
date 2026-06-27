using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vehicles.Dtos;

namespace Fgs.Setup.Application.Abstractions.Vehicles;

public interface IFgsVehicleReadRepository
{
    Task<FgsVehicleDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsVehicleSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsVehicleListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsVehicleLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsInventoryLocationIdAsync(
        long id,
        CancellationToken cancellationToken = default);
}
