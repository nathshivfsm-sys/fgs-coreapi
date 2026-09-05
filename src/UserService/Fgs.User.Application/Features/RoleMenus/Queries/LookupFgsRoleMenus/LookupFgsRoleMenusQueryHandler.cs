using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Queries.LookupFgsRoleMenus;

public sealed class LookupFgsRoleMenusQueryHandler(IFgsRoleMenuReadRepository readRepository)
    : IRequestHandler<LookupFgsRoleMenusQuery, ApiResponse<IReadOnlyList<FgsRoleMenuLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRoleMenuLookupDto>>> Handle(
        LookupFgsRoleMenusQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.RoleId, request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsRoleMenuLookupDto>>.Ok(result);
    }
}
