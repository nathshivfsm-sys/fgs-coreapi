using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.GetUserIdsByRoles;

public sealed class GetUserIdsByRolesQueryHandler(IFgsUserReadRepository readRepository)
    : IRequestHandler<GetUserIdsByRolesQuery, ApiResponse<IReadOnlyList<Guid>>>
{
    public async Task<ApiResponse<IReadOnlyList<Guid>>> Handle(
        GetUserIdsByRolesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.RoleIds is not { Count: > 0 })
        {
            return ApiResponse<IReadOnlyList<Guid>>.Ok([]);
        }

        var userIds = await readRepository.GetIdsByRoleIdsAsync(request.RoleIds, cancellationToken);
        return ApiResponse<IReadOnlyList<Guid>>.Ok(userIds);
    }
}
