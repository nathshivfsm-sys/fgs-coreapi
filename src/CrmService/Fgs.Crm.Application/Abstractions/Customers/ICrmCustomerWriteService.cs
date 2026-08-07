using Fgs.Crm.Application.Features.Customers.Dtos;

namespace Fgs.Crm.Application.Abstractions.Customers;

public interface ICrmCustomerWriteService
{
    Task<CrmCustomerDetailDto> CreateAsync(CrmCustomerCreateDto dto, CancellationToken cancellationToken = default);

    Task<CrmCustomerDetailDto> UpdateAsync(long id, CrmCustomerUpdateDto dto, CancellationToken cancellationToken = default);

    Task<CrmCustomerDetailDto> PatchAsync(long id, CrmCustomerPatchDto dto, CancellationToken cancellationToken = default);
}
