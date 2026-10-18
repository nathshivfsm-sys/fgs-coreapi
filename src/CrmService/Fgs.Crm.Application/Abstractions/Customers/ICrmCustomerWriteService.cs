using Fgs.Crm.Application.Features.Customers.Dtos;

namespace Fgs.Crm.Application.Abstractions.Customers;

public interface ICrmCustomerWriteService
{
    Task<CrmCustomerCreateResultDto> CreateAsync(CrmCustomerCreateDto dto, CancellationToken cancellationToken = default);

    Task<CrmServiceLocationCreatedDto> AddServiceLocationAsync(
        long customerId,
        CrmServiceLocationCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<CrmServiceLocationDetailDto> UpdateServiceLocationAsync(
        long customerId,
        long serviceLocationId,
        CrmServiceLocationUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<CrmCustomerDetailDto> UpdateAsync(long id, CrmCustomerUpdateDto dto, CancellationToken cancellationToken = default);

    Task<CrmCustomerDetailDto> PatchAsync(long id, CrmCustomerPatchDto dto, CancellationToken cancellationToken = default);

    Task<CrmDefaultCustomerDto> UpsertDefaultCustomerAsync(
        CrmDefaultCustomerUpsertDto dto,
        CancellationToken cancellationToken = default);

    Task<CrmDefaultServiceLocationDto> UpsertDefaultServiceLocationAsync(
        CrmDefaultServiceLocationUpsertDto dto,
        CancellationToken cancellationToken = default);
}
