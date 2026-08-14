using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Queries.ListFgsRoleDataAccessesByRoleId;

public sealed class ListFgsRoleDataAccessesByRoleIdQueryHandler(IFgsRoleDataAccessReadRepository readRepository)
    : IRequestHandler<ListFgsRoleDataAccessesByRoleIdQuery, ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>> Handle(
        ListFgsRoleDataAccessesByRoleIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListByRoleIdAsync(request.FgsRoleId, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>.Ok(result);
    }
}
