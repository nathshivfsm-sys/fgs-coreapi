using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.ListFgsUsers;

public sealed class ListFgsUsersQueryHandler(IFgsUserReadRepository readRepository)
    : IRequestHandler<ListFgsUsersQuery, ApiResponse<PagedResult<FgsUserSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUserSummaryDto>>> Handle(
        ListFgsUsersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsUserSummaryDto>>.Ok(result);
    }
}
