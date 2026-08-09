using Fgs.Contracts.Api;
using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Billing.Application.Features.Invoices.Queries.ListFgsInvoices;

public sealed class ListFgsInvoicesQueryHandler(IFgsInvoiceReadRepository readRepository)
    : IRequestHandler<ListFgsInvoicesQuery, ApiResponse<PagedResult<FgsInvoiceSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInvoiceSummaryDto>>> Handle(
        ListFgsInvoicesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsInvoiceSummaryDto>>.Ok(result);
    }
}
