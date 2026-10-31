using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.DeleteCrmCustomerNote;

public sealed record DeleteCrmCustomerNoteCommand(long CustomerId, long NoteId)
    : IRequest<ApiResponse<CrmNoteDetailDto>>;
