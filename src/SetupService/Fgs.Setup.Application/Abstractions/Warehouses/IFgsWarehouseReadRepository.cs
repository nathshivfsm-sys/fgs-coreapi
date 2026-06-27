using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Warehouses.Dtos;

namespace Fgs.Setup.Application.Abstractions.Warehouses;

public interface IFgsWarehouseReadRepository
{
    Task<FgsWarehouseDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsWarehouseSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsWarehouseListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsWarehouseLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByWarehouseCodeAsync(
        string warehouseCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
