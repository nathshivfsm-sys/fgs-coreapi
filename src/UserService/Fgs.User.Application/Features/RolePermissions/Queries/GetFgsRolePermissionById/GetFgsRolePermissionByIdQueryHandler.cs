using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Queries.GetFgsRolePermissionById;

public sealed class GetFgsRolePermissionByIdQueryHandler(IFgsRolePermissionReadRepository readRepository)
    : IRequestHandler<GetFgsRolePermissionByIdQuery, ApiResponse<FgsRolePermissionDetailDto>>
{
    public async Task<ApiResponse<FgsRolePermissionDetailDto>> Handle(
        GetFgsRolePermissionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsRolePermissionDetailDto>.Fail(
                [$"Role permission assignment '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsRolePermissionDetailDto>.Ok(result);
    }
}
