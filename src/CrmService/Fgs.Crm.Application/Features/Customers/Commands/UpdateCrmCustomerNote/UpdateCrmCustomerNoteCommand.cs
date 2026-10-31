using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomerNote;

public sealed record UpdateCrmCustomerNoteCommand(
    long CustomerId,
    long NoteId,
    CrmNoteUpdateDto Dto)
    : IRequest<ApiResponse<CrmNoteDetailDto>>;
