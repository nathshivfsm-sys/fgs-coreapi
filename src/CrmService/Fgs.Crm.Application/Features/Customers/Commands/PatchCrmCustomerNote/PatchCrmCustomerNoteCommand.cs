using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.PatchCrmCustomerNote;

public sealed record PatchCrmCustomerNoteCommand(
    long CustomerId,
    long NoteId,
    CrmNotePatchDto Dto)
    : IRequest<ApiResponse<CrmNoteDetailDto>>;
