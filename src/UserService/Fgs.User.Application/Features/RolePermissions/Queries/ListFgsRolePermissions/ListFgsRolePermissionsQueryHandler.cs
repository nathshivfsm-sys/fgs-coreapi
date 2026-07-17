using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Queries.ListFgsRolePermissions;

public sealed class ListFgsRolePermissionsQueryHandler(IFgsRolePermissionReadRepository readRepository)
    : IRequestHandler<ListFgsRolePermissionsQuery, ApiResponse<PagedResult<FgsRolePermissionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsRolePermissionSummaryDto>>> Handle(
        ListFgsRolePermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsRolePermissionSummaryDto>>.Ok(result);
    }
}
