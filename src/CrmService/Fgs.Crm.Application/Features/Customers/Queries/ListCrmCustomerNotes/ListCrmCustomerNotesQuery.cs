using Fgs.Contracts.Api;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomerNotes;

public sealed record ListCrmCustomerNotesQuery(
    long CustomerId,
    CrmListQuery Query,
    CrmNoteListFilters Filters)
    : IRequest<ApiResponse<CrmNoteListResultDto>>;
