using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vendors.Dtos;

namespace Fgs.Setup.Application.Abstractions.Vendors;

public interface IFgsVendorReadRepository
{
    Task<FgsVendorDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsVendorSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsVendorListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsVendorLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByVendorCodeAsync(
        string vendorCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsPaymentTermIdAsync(
        long? id,
        CancellationToken cancellationToken = default);
}
