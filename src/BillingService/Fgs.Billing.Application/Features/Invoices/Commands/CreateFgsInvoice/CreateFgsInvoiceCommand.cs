using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.Invoices.Commands.CreateFgsInvoice;

public sealed record CreateFgsInvoiceCommand(FgsInvoiceCreateDto Dto)
    : IRequest<ApiResponse<FgsInvoiceDetailDto>>;
