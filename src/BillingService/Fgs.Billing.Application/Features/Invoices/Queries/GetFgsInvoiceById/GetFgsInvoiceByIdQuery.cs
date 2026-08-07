using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.Invoices.Queries.GetFgsInvoiceById;

public sealed record GetFgsInvoiceByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInvoiceDetailDto>>;
