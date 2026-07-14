using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Queries.GetFgsRoleDataAccessById;

public sealed class GetFgsRoleDataAccessByIdQueryHandler(IFgsRoleDataAccessReadRepository readRepository)
    : IRequestHandler<GetFgsRoleDataAccessByIdQuery, ApiResponse<FgsRoleDataAccessDetailDto>>
{
    public async Task<ApiResponse<FgsRoleDataAccessDetailDto>> Handle(
        GetFgsRoleDataAccessByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsRoleDataAccessDetailDto>.Fail(
                [$"Role data access assignment '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsRoleDataAccessDetailDto>.Ok(result);
    }
}
