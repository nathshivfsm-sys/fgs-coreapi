using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.AddCrmCustomerNote;

public sealed record AddCrmCustomerNoteCommand(long CustomerId, CrmNoteCreateDto Dto)
    : IRequest<ApiResponse<CrmNoteDetailDto>>;
