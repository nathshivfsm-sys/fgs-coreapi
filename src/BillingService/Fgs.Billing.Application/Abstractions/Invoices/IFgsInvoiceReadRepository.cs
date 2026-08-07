using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Foundation.Paging;

namespace Fgs.Billing.Application.Abstractions.Invoices;

public interface IFgsInvoiceReadRepository
{
    Task<FgsInvoiceDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsInvoiceSummaryDto>> ListAsync(
        BillingListQuery query,
        FgsInvoiceListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsInvoiceLookupDto>> LookupAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsByInvoiceNumberAsync(
        string invoiceNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}
