using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Queries.GetFgsUserRoleById;

public sealed class GetFgsUserRoleByIdQueryHandler(IFgsUserRoleReadRepository readRepository)
    : IRequestHandler<GetFgsUserRoleByIdQuery, ApiResponse<FgsUserRoleDetailDto>>
{
    public async Task<ApiResponse<FgsUserRoleDetailDto>> Handle(
        GetFgsUserRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsUserRoleDetailDto>.Fail(
                [$"User role assignment '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsUserRoleDetailDto>.Ok(result);
    }
}
