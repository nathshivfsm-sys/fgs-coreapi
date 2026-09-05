using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Queries.LookupFgsUserRoles;

public sealed class LookupFgsUserRolesQueryHandler(IFgsUserRoleReadRepository readRepository)
    : IRequestHandler<LookupFgsUserRolesQuery, ApiResponse<IReadOnlyList<FgsUserRoleLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUserRoleLookupDto>>> Handle(
        LookupFgsUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.UserId, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsUserRoleLookupDto>>.Ok(result);
    }
}
