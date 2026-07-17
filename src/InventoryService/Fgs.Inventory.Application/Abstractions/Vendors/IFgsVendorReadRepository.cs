using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.Vendors.Dtos;

namespace Fgs.Inventory.Application.Abstractions.Vendors;

public interface IFgsVendorReadRepository
{
    Task<FgsVendorDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsVendorSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsVendorListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsVendorLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByVendorCodeAsync(
        string vendorCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
