using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Queries.LookupFgsRoleDataAccesses;

public sealed class LookupFgsRoleDataAccessesQueryHandler(IFgsRoleDataAccessReadRepository readRepository)
    : IRequestHandler<LookupFgsRoleDataAccessesQuery, ApiResponse<IReadOnlyList<FgsRoleDataAccessLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRoleDataAccessLookupDto>>> Handle(
        LookupFgsRoleDataAccessesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.FgsRoleId, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsRoleDataAccessLookupDto>>.Ok(result);
    }
}
