using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerNoteById;

public sealed class GetCrmCustomerNoteByIdQueryHandler(ICrmCustomerReadRepository readRepository)
    : IRequestHandler<GetCrmCustomerNoteByIdQuery, ApiResponse<CrmNoteDetailDto>>
{
    public async Task<ApiResponse<CrmNoteDetailDto>> Handle(
        GetCrmCustomerNoteByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetNoteByIdAsync(
            request.CustomerId,
            request.NoteId,
            cancellationToken);

        if (result is null)
        {
            return ApiResponse<CrmNoteDetailDto>.Fail(
                [$"Note '{request.NoteId}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<CrmNoteDetailDto>.Ok(result);
    }
}
