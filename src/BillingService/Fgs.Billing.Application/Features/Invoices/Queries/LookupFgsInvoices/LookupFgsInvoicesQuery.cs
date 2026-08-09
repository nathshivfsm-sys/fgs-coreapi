using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.Invoices.Queries.LookupFgsInvoices;

public sealed record LookupFgsInvoicesQuery()
    : IRequest<ApiResponse<IReadOnlyList<FgsInvoiceLookupDto>>>;
