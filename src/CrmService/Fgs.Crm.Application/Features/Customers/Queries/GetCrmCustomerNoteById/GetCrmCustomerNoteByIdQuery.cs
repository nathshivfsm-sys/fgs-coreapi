using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerNoteById;

public sealed record GetCrmCustomerNoteByIdQuery(long CustomerId, long NoteId)
    : IRequest<ApiResponse<CrmNoteDetailDto>>;
