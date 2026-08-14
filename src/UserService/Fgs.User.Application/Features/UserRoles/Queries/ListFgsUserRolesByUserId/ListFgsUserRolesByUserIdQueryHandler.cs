using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Queries.ListFgsUserRolesByUserId;

public sealed class ListFgsUserRolesByUserIdQueryHandler(IFgsUserRoleReadRepository readRepository)
    : IRequestHandler<ListFgsUserRolesByUserIdQuery, ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>> Handle(
        ListFgsUserRolesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListByUserIdAsync(request.UserId, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>.Ok(result);
    }
}
