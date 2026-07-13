using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Queries.LookupFgsRoles;

public sealed class LookupFgsRolesQueryHandler(IFgsRoleReadRepository readRepository)
    : IRequestHandler<LookupFgsRolesQuery, ApiResponse<IReadOnlyList<FgsRoleLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRoleLookupDto>>> Handle(
        LookupFgsRolesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsRoleLookupDto>>.Ok(result);
    }
}
