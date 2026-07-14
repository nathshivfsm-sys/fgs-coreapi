using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Permissions.Queries.GetFgsPermissionById;

public sealed class GetFgsPermissionByIdQueryHandler(IFgsPermissionReadRepository readRepository)
    : IRequestHandler<GetFgsPermissionByIdQuery, ApiResponse<FgsPermissionDetailDto>>
{
    public async Task<ApiResponse<FgsPermissionDetailDto>> Handle(
        GetFgsPermissionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsPermissionDetailDto>.Fail(
                [$"Permission '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsPermissionDetailDto>.Ok(result);
    }
}
