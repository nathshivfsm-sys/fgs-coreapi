using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Queries.ListFgsRolePermissionsByRoleId;

public sealed class ListFgsRolePermissionsByRoleIdQueryHandler(IFgsRolePermissionReadRepository readRepository)
    : IRequestHandler<ListFgsRolePermissionsByRoleIdQuery, ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>> Handle(
        ListFgsRolePermissionsByRoleIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListByRoleIdAsync(request.FgsRoleId, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>.Ok(result);
    }
}
