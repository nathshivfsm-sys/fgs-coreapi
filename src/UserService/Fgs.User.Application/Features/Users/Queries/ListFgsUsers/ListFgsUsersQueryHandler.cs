using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.ListFgsUsers;

public sealed class ListFgsUsersQueryHandler(IFgsUserReadRepository readRepository)
    : IRequestHandler<ListFgsUsersQuery, ApiResponse<FgsUserListResultDto>>
{
    public async Task<ApiResponse<FgsUserListResultDto>> Handle(
        ListFgsUsersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(
            request.Query,
            request.Filters,
            request.IncludeSummary,
            cancellationToken);
        return ApiResponse<FgsUserListResultDto>.Ok(result);
    }
}
