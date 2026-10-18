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

    Task<CrmCompanyServiceLocationListResultDto> ListCompanyServiceLocationsAsync(
        CrmListQuery query,
        CrmCompanyServiceLocationListFilters filters,
        bool includeSummary = true,
        CancellationToken cancellationToken = default);

    Task<CrmServiceLocationDetailDto?> GetServiceLocationByIdAsync(
        long customerId,
        long serviceLocationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CrmCustomerLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCustomerNumberAsync(
        string customerNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default);

    Task<CrmDefaultCustomerDto?> GetDefaultCustomerAsync(CancellationToken cancellationToken = default);

    Task<CrmDefaultServiceLocationDto?> GetDefaultServiceLocationAsync(CancellationToken cancellationToken = default);
}
