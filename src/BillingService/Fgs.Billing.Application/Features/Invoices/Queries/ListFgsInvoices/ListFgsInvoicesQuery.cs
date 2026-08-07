using Fgs.Contracts.Api;
using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Billing.Application.Features.Invoices.Queries.ListFgsInvoices;

public sealed record ListFgsInvoicesQuery(
    BillingListQuery Query,
    FgsInvoiceListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInvoiceSummaryDto>>>;
