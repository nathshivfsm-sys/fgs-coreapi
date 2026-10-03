using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;

namespace Fgs.Crm.Application.Abstractions.Customers;

public interface ICrmCustomerReadRepository
{
    Task<CrmCustomerDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<CrmCustomerListResultDto> ListAsync(
        CrmListQuery query,
        CrmCustomerListFilters filters,
        bool includeSummary = true,
        CancellationToken cancellationToken = default);

    Task<CrmServiceLocationListResultDto?> ListServiceLocationsAsync(
        long customerId,
        CrmListQuery query,
        bool includeSummary = true,
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
