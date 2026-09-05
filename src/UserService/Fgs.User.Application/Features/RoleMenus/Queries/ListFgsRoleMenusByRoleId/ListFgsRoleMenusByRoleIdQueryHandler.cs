using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Queries.ListFgsRoleMenusByRoleId;

public sealed class ListFgsRoleMenusByRoleIdQueryHandler(IFgsRoleMenuReadRepository readRepository)
    : IRequestHandler<ListFgsRoleMenusByRoleIdQuery, ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>> Handle(
        ListFgsRoleMenusByRoleIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListByRoleIdAsync(request.RoleId, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>.Ok(result);
    }
}
