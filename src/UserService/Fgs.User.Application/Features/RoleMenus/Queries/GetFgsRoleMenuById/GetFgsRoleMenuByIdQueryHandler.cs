using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Queries.GetFgsRoleMenuById;

public sealed class GetFgsRoleMenuByIdQueryHandler(IFgsRoleMenuReadRepository readRepository)
    : IRequestHandler<GetFgsRoleMenuByIdQuery, ApiResponse<FgsRoleMenuDetailDto>>
{
    public async Task<ApiResponse<FgsRoleMenuDetailDto>> Handle(
        GetFgsRoleMenuByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsRoleMenuDetailDto>.Fail(
                [$"Role menu '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsRoleMenuDetailDto>.Ok(result);
    }
}
