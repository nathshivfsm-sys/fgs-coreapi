using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomerNotes;

public sealed class ListCrmCustomerNotesQueryHandler(ICrmCustomerReadRepository readRepository)
    : IRequestHandler<ListCrmCustomerNotesQuery, ApiResponse<CrmNoteListResultDto>>
{
    public async Task<ApiResponse<CrmNoteListResultDto>> Handle(
        ListCrmCustomerNotesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListNotesAsync(
            request.CustomerId,
            request.Query,
            request.Filters,
            cancellationToken);

        if (result is null)
        {
            return ApiResponse<CrmNoteListResultDto>.Fail(
                [$"Customer '{request.CustomerId}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<CrmNoteListResultDto>.Ok(result);
    }
}
