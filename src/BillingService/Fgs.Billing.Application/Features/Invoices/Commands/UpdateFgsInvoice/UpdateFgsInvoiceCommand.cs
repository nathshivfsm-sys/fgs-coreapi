using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.Invoices.Commands.UpdateFgsInvoice;

public sealed record UpdateFgsInvoiceCommand(long Id, FgsInvoiceUpdateDto Dto)
    : IRequest<ApiResponse<FgsInvoiceDetailDto>>;
