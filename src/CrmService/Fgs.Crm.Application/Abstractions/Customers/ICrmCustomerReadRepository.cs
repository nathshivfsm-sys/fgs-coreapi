using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Paging;

namespace Fgs.Crm.Application.Abstractions.Customers;

public interface ICrmCustomerReadRepository
{
    Task<CrmCustomerDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<CrmCustomerSummaryDto>> ListAsync(
        CrmListQuery query,
        CrmCustomerListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CrmCustomerLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCustomerNumberAsync(
        string customerNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default);
}
