using Fgs.Billing.Application.Features.Invoices.Dtos;

namespace Fgs.Billing.Application.Abstractions.Invoices;

public interface IFgsInvoiceWriteService
{
    Task<FgsInvoiceDetailDto> CreateAsync(FgsInvoiceCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInvoiceDetailDto> UpdateAsync(long id, FgsInvoiceUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsInvoiceDetailDto> PatchAsync(long id, FgsInvoicePatchDto dto, CancellationToken cancellationToken = default);
}
