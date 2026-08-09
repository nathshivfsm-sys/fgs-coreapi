using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.Invoices.Commands.PatchFgsInvoice;

public sealed record PatchFgsInvoiceCommand(long Id, FgsInvoicePatchDto Dto)
    : IRequest<ApiResponse<FgsInvoiceDetailDto>>;
