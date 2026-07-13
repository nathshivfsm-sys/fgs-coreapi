using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Queries.GetFgsRoleById;

public sealed class GetFgsRoleByIdQueryHandler(IFgsRoleReadRepository readRepository)
    : IRequestHandler<GetFgsRoleByIdQuery, ApiResponse<FgsRoleDetailDto>>
{
    public async Task<ApiResponse<FgsRoleDetailDto>> Handle(
        GetFgsRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsRoleDetailDto>.Fail(
                [$"Role '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsRoleDetailDto>.Ok(result);
    }
}
