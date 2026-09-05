using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Queries.LookupFgsRolePermissions;

public sealed class LookupFgsRolePermissionsQueryHandler(IFgsRolePermissionReadRepository readRepository)
    : IRequestHandler<LookupFgsRolePermissionsQuery, ApiResponse<IReadOnlyList<FgsRolePermissionLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRolePermissionLookupDto>>> Handle(
        LookupFgsRolePermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.FgsRoleId, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsRolePermissionLookupDto>>.Ok(result);
    }
}
